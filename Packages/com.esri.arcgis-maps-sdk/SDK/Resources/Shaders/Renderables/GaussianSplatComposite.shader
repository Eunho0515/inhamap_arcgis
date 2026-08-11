Shader "Custom/GaussianSplatComposite"
{
	HLSLINCLUDE
    #include "UnityCG.cginc"

    struct VS_OUTPUT
    {
        float4 position : SV_POSITION;
        float2 uv : TEXCOORD0;
    };

    struct FS_OUTPUT
    {
        float4 color : SV_Target0;
        float depth : SV_Depth;
    };

    Texture2D<float4> _AccumulatedColorTexture;
    Texture2D<float4> _AccumulatedDepthAlphaTexture;
    SamplerState sampler_AccumulatedColorTexture;
    SamplerState sampler_AccumulatedDepthAlphaTexture;

    VS_OUTPUT compose_vert(uint vertexID : SV_VertexID)
    {
        VS_OUTPUT o;
        float2 uv = float2((vertexID << 1) & 2, vertexID & 2);
        o.position = float4(uv * 2.0 - 1.0, 0.0, 1.0);
        o.uv = uv;
        return o;
    }

    FS_OUTPUT compose_frag(VS_OUTPUT i)
    {
        FS_OUTPUT o;

        float2 textureCoordinate = i.uv;

#if UNITY_UV_STARTS_AT_TOP
        if (_ProjectionParams.x < 0.0)
        {
            textureCoordinate.y = 1.0 - textureCoordinate.y;
        }
#endif

        float4 accumulatedColor = _AccumulatedColorTexture.Sample(sampler_AccumulatedColorTexture, textureCoordinate);
        float4 depthAlpha = _AccumulatedDepthAlphaTexture.Sample(sampler_AccumulatedDepthAlphaTexture, textureCoordinate);

        if (accumulatedColor.a < 0.3)
        {
            depthAlpha.x = 0.0;
        }

        o.color = accumulatedColor;
        o.depth = saturate(depthAlpha.x);

        return o;
    }
	ENDHLSL

	SubShader
	{
		Tags { "RenderType"="TransparentCutout" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
		LOD 100
		Cull Off

		Pass
		{
			Name "Composite"
			Tags { "LightMode"="UniversalForward" }
			Blend One OneMinusSrcAlpha
			ZWrite On
            ZTest Always

			HLSLPROGRAM
			#pragma target 4.5
			#pragma vertex compose_vert
			#pragma fragment compose_frag
			ENDHLSL
		}
	}

	SubShader
	{
		Tags { "RenderType"="TransparentCutout" "Queue"="Transparent" "RenderPipeline"="HDRenderPipeline" }
		LOD 100
		Cull Off

		Pass
		{
			Name "Composite"
			Tags { "LightMode"="ForwardOnly" }
			Blend One OneMinusSrcAlpha
			ZWrite On
            ZTest Always

			HLSLPROGRAM
			#pragma target 4.5
			#pragma vertex compose_vert
			#pragma fragment compose_frag
			ENDHLSL
		}
	}

    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="Transparent" }
        LOD 100
        Cull Off

        Pass
        {
            Name "Composite"
            Blend One OneMinusSrcAlpha
            ZWrite On
            ZTest Always

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex compose_vert
            #pragma fragment compose_frag
            ENDHLSL
        }
    }
}
