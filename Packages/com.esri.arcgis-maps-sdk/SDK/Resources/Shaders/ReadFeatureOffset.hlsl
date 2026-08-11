// Calculate texel to sample given offsets texture and feature index
int3 _getFeatureOffsetsTexelAddress(Texture2D featureOffsets, float featureIndex)
{
  // Remove fractional error
  featureIndex = round(featureIndex);

  // Get the feature offsets texture dimensions
  uint width, height;
  featureOffsets.GetDimensions(width, height);

  int y = int(floor(featureIndex / width));
  int x = int(featureIndex - y * width);

  return int3(x, y, 0);
}

// Return the value of the texel at index featureIndex, where addressing is by row starting from 0,0
void ReadFeatureOffset_float(
  Texture2D featureOffsets,
  float featureIndex,
  out float3 featureOffsetValue)
{
  featureOffsetValue = featureOffsets.Load(_getFeatureOffsetsTexelAddress(featureOffsets, featureIndex)).xyz;
}
