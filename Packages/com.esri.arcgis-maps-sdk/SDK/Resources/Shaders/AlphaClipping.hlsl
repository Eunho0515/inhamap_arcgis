void AlphaClipping_float(float3 worldPosition, float4x4 worldToObjectMatrix, int clippingMode, float3 mapAreaMin, float3 mapAreaMax, out float alpha)
{
	if (clippingMode == 1)
	{
		float3 localPosition = mul(worldToObjectMatrix, float4(worldPosition, 1.0f)).xyz;

		float radius = (mapAreaMax.x - mapAreaMin.x) * 0.5f;
		float3 center = (mapAreaMin + mapAreaMax) * 0.5f;

		localPosition.y = mapAreaMin.y;

		alpha = distance(center, localPosition) < radius ? 1.0f : 0.0f;
	}
	else if (clippingMode == 2)
	{
		float3 localPosition = mul(worldToObjectMatrix, float4(worldPosition, 1.0f)).xyz;

		localPosition.y = mapAreaMin.y;

		alpha = mapAreaMin.x < localPosition.x && mapAreaMin.z < localPosition.z && mapAreaMax.x > localPosition.x && mapAreaMax.z > localPosition.z ? 1.0f : 0.0f;
	}
	else
	{
		alpha = 1.0f;
	}
}

void InstancedAlphaClipping_float(float3 worldPosition, float4x4 worldToBatchRootMatrix, int clippingMode, float3 mapAreaMin, float3 mapAreaMax, out float alpha)
{
	#if defined(SHADEROPTIONS_CAMERA_RELATIVE_RENDERING) && (SHADEROPTIONS_CAMERA_RELATIVE_RENDERING != 0)
		worldPosition += _WorldSpaceCameraPos.xyz;
	#endif

	AlphaClipping_float(worldPosition, worldToBatchRootMatrix, clippingMode, mapAreaMin, mapAreaMax, alpha);
}
