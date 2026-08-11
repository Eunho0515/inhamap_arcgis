#include "BillboardMarkerCommon.hlsl"

struct VS_output
{
	float4 position : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 worldPosition : TEXCOORD1;
	float4 offsetSize : TEXCOORD2;
	float2 centerScreenUV : TEXCOORD3;
	float4 color : COLOR;
};
VS_output vert(appdata v)
{
	InitIndirectDrawArgs(0);

	VS_output output;

	InstanceData instanceData = _InstanceBuffer[v.instanceID];

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

	output.uv = float2(v.vertex.x, v.vertex.y) + 0.5;
	output.worldPosition = worldPos.xyz;
	output.offsetSize = UnpackUVRegion(instanceData.uvRegion);
	output.centerScreenUV = GetBillboardMarkerCenterScreenUV(worldPos);

	output.color = UnpackColor(instanceData.color);
	output.color.a *= _Opacity;

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
	float4 mainTexColor = tex2D(_MainTex, atlasUV);
	float alpha = mainTexColor.w;

	float4 colorValue = float4(mainTexColor.rgb * input.color.rgb, alpha * input.color.a);
	ApplyBillboardMarkerSceneDepthClip(input.centerScreenUV, input.worldPosition);

	return colorValue;
}
