#include "BillboardMarkerCommon.hlsl"

// Bit-mask helpers to check whether bit 1 or 2 are set on v
#define TEST_BIT1(v) ((((uint)(v)) & 2u) != 0u)
#define TEST_BIT2(v) ((((uint)(v)) & 4u) != 0u)

struct VS_output
{
	float4 position : SV_POSITION;
	float2 uv : TEXCOORD0;
	float4 offsetSize : TEXCOORD1;
	float4 fillColor : TEXCOORD2;
	float4 strokeColor : TEXCOORD3;
	float flowFactor : TEXCOORD4;
	float dashFraction : TEXCOORD5;
	float scaleSdf : TEXCOORD6;
	float lineHalfWidth : TEXCOORD7;
	float3 worldPosition : TEXCOORD8;
	float2 centerScreenUV : TEXCOORD9;
};

struct InstanceDynamicData
{
	// RGB for stroke color
	// A for stroke fraction
	uint color;

	// RG = uint16 sdf_to_screen * 200
	// B = sdf line width
	// A0 honor reference scale
	// A1 proportional line scaling
	// A2 fill in content as a polygon
	// A4-7 flow factor (for dashed lines)
	uint dynamicBillboardParams;

	// R = unused
	// GB = 2 * halo size
	// A0 = text decorator flag
	// A1 = halo mode flag
	// A2-7 = unused
	uint haloColorDecoratorFlags;
};
StructuredBuffer<InstanceDynamicData> _InstanceDynamicBuffer;

// SDF scale is stored as a 16-bit fixed-point value split across
// the R (low byte) and G (high byte) channels.
// Encoding:
//     stored = sdfScale * 200
// Decoding:
//     sdfScale = ((G << 8) | R) / 200
// The factor of 200 provides 0.005 units of precision while fitting
// the value into two 8-bit channels.
float decodeSdfScale(float2 ax)
{
	return (ax.g * 256.0 + ax.r) / 200.0;
}

// Flow factor is stored in the upper 4 bits of the alpha byte.
// The renderer maps these 16 discrete values to the range [8.5, 136.0]
// using a fixed step size of 8.5
// CPU encoding:
//     stored = clamp(flowFactor - 1, 0, 15)
// Shader decoding:
//     flowFactor = 8.5 * (stored + 1)
float decodeFlowFactor(float ax)
{
	return 8.5 * (1.0 + ax);
}

// Halo width is encoded in a 16-bit range using the G and B channels of the haloColorDecoratorFlags uint.
// The decoding function converts this back to a float value in pixels and one decimal.
// Example (G = 200, B = 1): (1*256 + 200) / 10 = 45.6 pixels halo width.
float decodeHaloWidth(float2 ax)
{
	return ((ax.y * 256.0f) + ax.x) / 10.0f;
}

// Stroke width is stored as an 8-bit fixed-point value with one decimal
// digit of precision.
// CPU encoding:
//     stored = round(width * 10)
// Shader decoding:
//     width = stored / 10
float decodeStrokeWidth(float ax)
{
	return 0.1 * ax;
}

VS_output vert(appdata v)
{
	InitIndirectDrawArgs(0);

	VS_output output;

	InstanceData instanceData = _InstanceBuffer[v.instanceID];
	InstanceDynamicData instanceDynamicData = _InstanceDynamicBuffer[v.instanceID];

	bool useWorldSpace = _UseWorldSpaceUnits > 0.5;
	float4 worldPos = mul(_LocalToWorld, float4(instanceData.offset, 1.0));

	float2 sinCos = instanceData.sinCos;
	float2 anchor = instanceData.anchor;

	float2 position = CalculateRotatedPositionAroundAnchor(anchor, sinCos, float2(v.vertex.x, v.vertex.y));

	output.position = CalculateBillboardClipPosition(
		useWorldSpace,
		position,
		instanceData.size,
		worldPos,
		false);
	output.worldPosition = worldPos.xyz;
	output.centerScreenUV = GetBillboardMarkerCenterScreenUV(worldPos);

	float4 dynamicBillboardParams = UnpackBytesToFloat4(instanceDynamicData.dynamicBillboardParams);

	float4 fillColor = UnpackColor(instanceData.color);
	float opacity = fillColor.a * _Opacity;

	output.fillColor = float4(fillColor.rgb, float(TEST_BIT2(dynamicBillboardParams.a)) * opacity);

	float4 strokeColorAndFraction = UnpackColor(instanceDynamicData.color);
	output.strokeColor = float4(strokeColorAndFraction.rgb, opacity);

	output.flowFactor = decodeFlowFactor(floor(dynamicBillboardParams.a / 16.0f));
	output.dashFraction = strokeColorAndFraction.a;

	output.scaleSdf = decodeSdfScale(float2(dynamicBillboardParams.r, dynamicBillboardParams.g));

	float maxScreenLineWidth = output.scaleSdf * 0.5 - 0.5;
	float minScreenLineWidth = 0.5;

	float4 haloColorDecoratorFlags = UnpackBytesToFloat4(instanceDynamicData.haloColorDecoratorFlags);

	float strokeWidth = TEST_BIT1(haloColorDecoratorFlags.a) ? decodeHaloWidth(haloColorDecoratorFlags.gb) :
	decodeStrokeWidth(dynamicBillboardParams.b);
	output.lineHalfWidth = clamp(0.5 * strokeWidth, minScreenLineWidth, maxScreenLineWidth);

	output.uv = float2(v.vertex.x, v.vertex.y) + 0.5;
	output.offsetSize = UnpackUVRegion(instanceData.uvRegion);

	return output;
}

float4 frag(VS_output input) : SV_Target
{
	float clippingAlpha = 1.0;
	AlphaClipping_float(input.worldPosition, _WorldToLocal, _ClippingMode, _MapAreaMin, _MapAreaMax, clippingAlpha);
	clip(clippingAlpha - 0.5);

	float2 uv = saturate(input.uv);
	uv.y = 1.0 - uv.y;
	float2 atlasUV = input.offsetSize.xy + uv * input.offsetSize.zw;

	float4 msdf = tex2D(_MainTex, atlasUV);

	// Flow data is packed immediately to the right of the MSDF region in the atlas.
	// Keep a 2-pixel gap to avoid sampling across region edges.
	// Atlas textures are expected to be 1024x1024, so convert pixels to normalized UV.
	const float atlasTextureSize = 1024.0;
	const float antiBleedPixels = 2.0;
	const float antiBleedMargin = antiBleedPixels / atlasTextureSize;
	float2 flowUV = atlasUV + float2(input.offsetSize.z + antiBleedMargin, 0.0);
	float4 flow = tex2D(_MainTex, flowUV);

	// Calculate the sdf value in number of pixels from the 0.5 line
	float medianSdf = max(min(msdf.r, msdf.g), min(max(msdf.r, msdf.g), msdf.b));
	float sdf = (medianSdf - 0.5) * input.scaleSdf;

	// Give a one-pixel antialias to the sdf value
	float sdfAlpha = smoothstep(-0.5, 0.5, sdf);

	// Pick the flow value out of the flow texture
	float specificFlow = lerp(flow.g, flow.b, step(0.5, flow.a));

	// Calculate the dash pattern based on the dash fraction
	// If the dash fraction is zero, then no outline will show up
	float dashPattern = step(frac(specificFlow * input.flowFactor - 0.25), input.dashFraction);

	// If the rate of the specific_flow variable is approaching a single pixel, then dashes are going
	// to start aliasing. In this case, fade the outline to the dash_fraction value as alpha.
	float dashFadeout = smoothstep(0.5, 0.75f, fwidth(specificFlow) * input.flowFactor);
	float dashAlpha = lerp(dashPattern, input.dashFraction, dashFadeout);

	// Calculate where the edges of the outline fall clamped to the pixel vicinity of this sdf value
	float innerEdge = max(sdf - 0.5, -input.lineHalfWidth);
	float outerEdge = min(sdf + 0.5, input.lineHalfWidth);

	// Find the portion of the outline inside the sdf vicinity (to anti-alias the line)
	// At the same time, clip it to the outline clip channel, and factor in the dashes.
	float strokeAlpha = clamp(outerEdge - innerEdge, 0.0, 1.0) * step(0.5, flow.r) * dashAlpha;

	// Calculate final colors
	float4 fillValue = input.fillColor * sdfAlpha;
	float4 strokeValue = input.strokeColor * strokeAlpha;

	// Frag is the blending of stroke on top of fill.
	// Premultiply RGB so it matches Blend One OneMinusSrcAlpha.
	float4 colorValue = (1.0 - strokeValue.a) * fillValue + strokeValue;
	colorValue.rgb *= colorValue.a;
	ApplyBillboardMarkerSceneDepthClip(input.centerScreenUV, input.worldPosition);

	return colorValue;
}
