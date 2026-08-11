void UVRegionTransform_float(float2 uv, Texture2D lut, float lutIndex, float useUVRegionLUT, out float2 outUV)
{
	if (useUVRegionLUT != 1.0)
	{
		outUV = uv;
	}
	else
	{
		float4 uvRegion = lut.Load(int3((uint)(lutIndex + 0.5f), 0, 0));

		outUV = frac(uv) * (uvRegion.zw - uvRegion.xy) + uvRegion.xy;
	}
}
