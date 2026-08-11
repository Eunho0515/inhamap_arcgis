#ifndef ARCGIS_BILLBOARD_MARKER_COMMON_INCLUDED
#define ARCGIS_BILLBOARD_MARKER_COMMON_INCLUDED

#include "UnityCG.cginc"
#define UNITY_INDIRECT_DRAW_ARGS IndirectDrawIndexedArgs
#include "UnityIndirect.cginc"
#include "../AlphaClipping.hlsl"
#include "../Billboard.hlsl"
#include "../Utils.hlsl"

struct appdata
{
	float4 vertex : POSITION;
	uint instanceID : SV_InstanceID;
};

struct InstanceData
{
	float2 size;
	float2 anchor;
	float2 sinCos;
	uint2 uvRegion;
	float3 offset;
	uint color;
};
StructuredBuffer<InstanceData> _InstanceBuffer;

sampler2D _MainTex;
#if defined(ARCGIS_RENDER_PIPELINE_HDRP)
	#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED) || \
        (defined(SHADER_API_D3D11) && !defined(SHADER_API_XBOXONE) && !defined(SHADER_API_GAMECORE)) || \
        defined(SHADER_API_PSSL) || defined(SHADER_API_VULKAN) || defined(SHADER_API_METAL)
		Texture2DArray<float> _CameraDepthTexture;
		#define ARCGIS_LOAD_CAMERA_DEPTH(pixelCoords) _CameraDepthTexture.Load(int4(pixelCoords, int(unity_StereoEyeIndex), 0))
	#else
		Texture2D<float> _CameraDepthTexture;
		#define ARCGIS_LOAD_CAMERA_DEPTH(pixelCoords) _CameraDepthTexture.Load(int3(pixelCoords, 0))
	#endif
	#define ARCGIS_SAMPLE_CAMERA_DEPTH(pixelCoords, screenUV) ARCGIS_LOAD_CAMERA_DEPTH(pixelCoords)
#elif defined(ARCGIS_RENDER_PIPELINE_UNIVERSAL) || defined(ARCGIS_RENDER_PIPELINE_BUILTIN)
	UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
	#define ARCGIS_SAMPLE_CAMERA_DEPTH(pixelCoords, screenUV) SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenUV)
#else
	#error "Unsupported render pipeline"
#endif

float4x4 _LocalToWorld;
float4x4 _WorldToLocal;
float _UseWorldSpaceUnits;
float _Opacity;
int _ClippingMode;
float3 _MapAreaMin;
float3 _MapAreaMax;

// Depth epsilon used when comparing marker center depth against sampled scene depth.
static const float ARCGIS_BILLBOARD_MARKER_DEPTH_BIAS = 1e-4;

float2 GetBillboardMarkerCenterScreenUV(float4 worldPosition)
{
	float4 centerClipPosition = mul(UNITY_MATRIX_VP, worldPosition);
	float4 centerScreenPosition = ComputeScreenPos(centerClipPosition);

	return centerScreenPosition.xy / centerScreenPosition.w;
}

// Returns marker center depth in [0, 1] using view-space z and Unity projection params.
// Keep this aligned with the scene depth conversion path used in fragment shaders.
float GetBillboardMarkerCenterDepth01(float4 worldPosition)
{
	float3 centerViewPosition = mul(UNITY_MATRIX_V, worldPosition).xyz;
	return saturate(-centerViewPosition.z * _ProjectionParams.w);
}

// Samples one raw depth texel by pixel coordinates; the backend differs per pipeline.
float SampleBillboardMarkerCameraDepthPixel(int2 pixelCoords)
{
	float2 screenUV = (float2(pixelCoords) + 0.5) / _ScreenParams.xy;

	return ARCGIS_SAMPLE_CAMERA_DEPTH(pixelCoords, screenUV);
}

// Sentinel value representing "no valid depth" for the current Z convention.
float GetBillboardMarkerEmptyCameraDepth()
{
	#if defined(UNITY_REVERSED_Z)
		return 0.0;
	#else
		return 1.0;
	#endif
}

// Treat near-0/near-1 values as empty background depending on Z convention.
bool IsBillboardMarkerCameraDepthEmpty(float screenDepth)
{
	#if defined(UNITY_REVERSED_Z)
		return screenDepth <= 0.000001;
	#else
		return screenDepth >= 0.999999;
	#endif
}

// IMPORTANT: "Farther" means lower depth in reversed-Z and higher depth otherwise.
bool IsBillboardMarkerCameraDepthFarther(float candidateDepth, float currentDepth)
{
	#if defined(UNITY_REVERSED_Z)
		return candidateDepth < currentDepth;
	#else
		return candidateDepth > currentDepth;
	#endif
}

// Samples a 5x5 neighborhood and returns the farthest valid scene depth.
// This intentionally favors far geometry to reduce accidental clipping against
// thin foreground depth edges around billboard markers.
float SampleBillboardMarkerFarthestCameraDepth(float2 screenUV)
{
	int2 screenSize = int2(_ScreenParams.xy);
	int2 basePixelCoords = int2(clamp(screenUV * _ScreenParams.xy, 0.0, _ScreenParams.xy - 1.0));

	float farthestDepth = GetBillboardMarkerEmptyCameraDepth();
	bool foundDepth = false;

	for (int y = -2; y <= 2; ++y)
	{
		for (int x = -2; x <= 2; ++x)
		{
			int2 pixelCoords = clamp(basePixelCoords + int2(x, y), int2(0, 0), screenSize - int2(1, 1));
			float depth = SampleBillboardMarkerCameraDepthPixel(pixelCoords);

			if (!IsBillboardMarkerCameraDepthEmpty(depth) && (!foundDepth || IsBillboardMarkerCameraDepthFarther(depth, farthestDepth)))
			{
				farthestDepth = depth;
				foundDepth = true;
			}
		}
	}

	return farthestDepth;
}

// Clips the current fragment if scene depth is closer than the billboard marker center depth.
void ApplyBillboardMarkerSceneDepthClip(float2 centerScreenUV, float3 worldPosition)
{
	float sceneDepth = SampleBillboardMarkerFarthestCameraDepth(centerScreenUV);
	float sceneDepth01 = Linear01Depth(sceneDepth);
	float markerDepth01 = GetBillboardMarkerCenterDepth01(float4(worldPosition, 1.0));

	// Small positive bias avoids precision flicker when marker and scene depths are nearly equal.
	clip(sceneDepth01 - markerDepth01 + ARCGIS_BILLBOARD_MARKER_DEPTH_BIAS);
}

#endif
