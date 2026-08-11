#ifndef UNITY_MATRIX_V
#define UNITY_MATRIX_V unity_MatrixV
#endif

#ifndef UNITY_MATRIX_P
#define UNITY_MATRIX_P unity_MatrixP
#endif

#ifndef UNITY_MATRIX_VP
#define UNITY_MATRIX_VP unity_MatrixVP
#endif

float2 CalculateRotatedPositionAroundAnchor(float2 anchor, float2 sinCos, float2 vertex)
{
	float2x2 rotationMatrix = float2x2(sinCos.y, -sinCos.x, sinCos.x, sinCos.y);
	return anchor + mul(rotationMatrix, vertex - anchor);
}

float4 CalculateWorldSpaceBillboardClipPosition(
	float2 billboardVertex,
	float2 billboardSize,
	float4 worldPosition,
	bool useMinMaxSize)
{
	float4 centerVS = mul(UNITY_MATRIX_V, worldPosition);
	float4 centerCS = mul(UNITY_MATRIX_P, centerVS);

	// Compute clip-space position for +1 view-space offset in X/Y.
	// This helps derive how many NDC units (i.e., screen pixels when scaled by w and ScreenParams)
	// correspond to one unit in view space along X and Y.
	float4 projXYClip = mul(UNITY_MATRIX_P, centerVS + float4(1.0, 1.0, 0.0, 0.0));
	// pixScaleNDC: NDC-space scale per +1 view-space unit in X/Y
	float2 pixScaleNDC = float2(
		abs(centerCS.x / centerCS.w - projXYClip.x / projXYClip.w),
		abs(centerCS.y / centerCS.w - projXYClip.y / projXYClip.w));

	float2 ptSizeNDC = billboardSize.xy * pixScaleNDC;

	if (useMinMaxSize)
	{
		// Max/min billboard size in pixels (converted to NDC via 2/screen):
		// max: 16 pixels wide/high, min: 2 pixels (ensures visibility and avoids oversized splats)
		float2 maxSizeNDC = 16.0 * (2.0 / _ScreenParams.xy);
		float2 minSizeNDC = 2.0 * (2.0 / _ScreenParams.xy);
		if (ptSizeNDC.x > maxSizeNDC.x || ptSizeNDC.y > maxSizeNDC.y)
		{
			ptSizeNDC = maxSizeNDC;
		}

		if (ptSizeNDC.x < minSizeNDC.x || ptSizeNDC.y < minSizeNDC.y)
		{
			ptSizeNDC = minSizeNDC;
		}
	}

	// Apply NDC offset and reconstruct clip-space position
	float4 clipPosition = centerCS / centerCS.w;
	clipPosition.xy += float2(billboardVertex.x * ptSizeNDC.x, -billboardVertex.y * ptSizeNDC.y);

	return clipPosition;
}

float4 CalculateScreenSpaceBillboardClipPosition(
	float2 billboardVertex,
	float2 billboardSize,
	float4 worldPosition)
{
	// Screen space
	float4 clipPosition = mul(UNITY_MATRIX_VP, worldPosition);

	// v.vertex defines a unit square with x/y in screen space coordinates
	float2 pixelToClip = (2.0 / _ScreenParams.xy) * clipPosition.w;
	clipPosition.xy += float2(billboardVertex.x * billboardSize.x, -billboardVertex.y * billboardSize.y) * pixelToClip;

	return clipPosition;
}

float4 CalculateBillboardClipPosition(
	bool useWorldSpace,
	float2 billboardVertex,
	float2 billboardSize,
	float4 worldPosition,
	bool useMinMaxSize)
{
	if (useWorldSpace)
	{
		return CalculateWorldSpaceBillboardClipPosition(
			billboardVertex,
			billboardSize,
			worldPosition,
			useMinMaxSize);
	}
	else
	{
		return CalculateScreenSpaceBillboardClipPosition(
			billboardVertex,
			billboardSize,
			worldPosition);
	}
}
