Shader "Custom/BillboardMarker"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_Opacity ("Opacity", Float) = 1
		_UseWorldSpaceUnits ("Use World Space Units", Float) = 0
		[HideInInspector] _ClippingMode ("Clipping Mode", Int) = 0
		[HideInInspector] _MapAreaMin ("Map Area Min", Vector) = (0, 0, 0, 0)
		[HideInInspector] _MapAreaMax ("Map Area Max", Vector) = (0, 0, 0, 0)
	}

	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
		LOD 100

		Pass
		{
			Tags { "LightMode"="UniversalForward" }
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha

			HLSLPROGRAM
			#define ARCGIS_RENDER_PIPELINE_UNIVERSAL
			#pragma vertex vert
			#pragma fragment frag
			#include "BillboardMarkerCore.hlsl"
			ENDHLSL
		}
	}

	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="HDRenderPipeline" }
		LOD 100

		Pass
		{
			Name "ForwardOnly"
			Tags { "LightMode"="ForwardOnly" }
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha

			HLSLPROGRAM
			#define ARCGIS_RENDER_PIPELINE_HDRP
			#define SHADERPASS SHADERPASS_FORWARD_UNLIT
			#pragma vertex vert
			#pragma fragment frag
			#include "BillboardMarkerCore.hlsl"
			ENDHLSL
		}
	}

	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100

		Pass
		{
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha

			HLSLPROGRAM
			#define ARCGIS_RENDER_PIPELINE_BUILTIN
			#pragma vertex vert
			#pragma fragment frag
			#include "BillboardMarkerCore.hlsl"
			ENDHLSL
		}
	}
}
