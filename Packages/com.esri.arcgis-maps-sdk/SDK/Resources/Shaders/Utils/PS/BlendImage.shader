Shader "Unlit/BlendImage"
{
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			#define NUM_TEXTURES_PER_PASS 7

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			int NumTextures;
			int OutputShouldBeSampled;

			Texture2D<float4> LastOutputRenderTexture;
			SamplerState samplerLastOutputRenderTexture;

			Texture2D<float4> Input0;
			SamplerState samplerInput0;
			Texture2D<float4> Input1;
			SamplerState samplerInput1;
			Texture2D<float4> Input2;
			SamplerState samplerInput2;
			Texture2D<float4> Input3;
			SamplerState samplerInput3;
			Texture2D<float4> Input4;
			SamplerState samplerInput4;
			Texture2D<float4> Input5;
			SamplerState samplerInput5;
			Texture2D<float4> Input6;
			SamplerState samplerInput6;

			float Opacities[NUM_TEXTURES_PER_PASS];
			float4 OffsetsAndScales[NUM_TEXTURES_PER_PASS];

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			#define SAMPLE_AND_BLEND(num) 																											\
				if (##num < NumTextures)																											\
				{																																	\
					texSample = Input##num.SampleLevel(samplerInput##num, input.uv * OffsetsAndScales[##num].z + OffsetsAndScales[##num].xy, 0);	\
					texSample = float4(LinearToGammaSpace(texSample.xyz), texSample.w);																\
					texSample *= Opacities[##num];																									\
					output = output * (1 - texSample.w) + texSample;																				\
				}

			fixed4 frag (v2f input) : SV_Target
			{
				float4 output = 1.0f;
				float4 texSample = 0.0f;

				if (OutputShouldBeSampled)
				{
					output = LastOutputRenderTexture.SampleLevel(samplerLastOutputRenderTexture, input.uv, 0);
					output = float4(LinearToGammaSpace(output.xyz), output.w);
				}

				SAMPLE_AND_BLEND(0);
				SAMPLE_AND_BLEND(1);
				SAMPLE_AND_BLEND(2);
				SAMPLE_AND_BLEND(3);
				SAMPLE_AND_BLEND(4);
				SAMPLE_AND_BLEND(5);
				SAMPLE_AND_BLEND(6);

				return float4(GammaToLinearSpace(output.xyz), output.w);
			}
			ENDCG
		}
	}
}
