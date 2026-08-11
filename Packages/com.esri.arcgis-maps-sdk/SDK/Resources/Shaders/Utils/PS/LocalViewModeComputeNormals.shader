Shader "Unlit/ComputeNormalsLocal"
{
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

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

			Texture2D<float> Input;

			SamplerState input_linear_clamp_sampler;

			float LatitudeLength;
			float LongitudeLength;
			float4 InputOffsetAndScale;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f input) : SV_Target
			{
				uint inputWidth, inputHeight;
				Input.GetDimensions(inputWidth, inputHeight);

				int2 id = int2(floor(input.uv * float2(inputWidth, inputHeight)));

				// Get 2 adjacent texel locations (x+1, y+1) from the curent texel location
				int2 sampleCoord0 = (int2) id.xy;
				int2 sampleCoord1 = sampleCoord0 + int2(1, 0);
				int2 sampleCoord2 = sampleCoord0 + int2(0, 1);

				// Calculate the inverse of the width and height of the input texture
				float2 invInputSize = float2(1.f / (float)inputWidth, 1.f / (float)inputHeight);

				// Calculate sampling locations based on the input texture size.
				// Half texel offset to sample the centre of a texel
				float2 sampleUV0 = InputOffsetAndScale.xy + (sampleCoord0 + 0.5f) * invInputSize * InputOffsetAndScale.zw;
				float2 sampleUV1 = InputOffsetAndScale.xy + (sampleCoord1 + 0.5f) * invInputSize * InputOffsetAndScale.zw;
				float2 sampleUV2 = InputOffsetAndScale.xy + (sampleCoord2 + 0.5f) * invInputSize * InputOffsetAndScale.zw;

				// Sample the input texture (elevation), taking into consideration the subregion of the texture
				float sample0 = Input.SampleLevel(input_linear_clamp_sampler, sampleUV0, 0);
				float sample1 = Input.SampleLevel(input_linear_clamp_sampler, sampleUV1, 0);
				float sample2 = Input.SampleLevel(input_linear_clamp_sampler, sampleUV2, 0);

				float latitudePixelSize = LatitudeLength * invInputSize.y;
				float longitudePixelSize = LongitudeLength * invInputSize.x;

				// Create vectors from the central pixel towards the neighbouring pixels
				float3 v0 = float3(longitudePixelSize, 0.0f, (sample1 - sample0));
				float3 v1 = float3(0.0f, latitudePixelSize, (sample2 - sample0));

				// Calculate the final normal vector, normalize it and handle edge cases (literally, texture edges)
				float3 normal = normalize(cross(v0, v1));

				// Remap to 0.0->1.0 range to fit RGBA8U texture
				return float4(0.5f * normal + 0.5f, 0.0f);
			}
			ENDCG
		}
	}
}
