// Calculate texel to sample given attribute texture and feature index
int3 _getAttributeTexelAddress(Texture2D attribute, float featureIndex)
{
  // Remove fractional error
  featureIndex = round(featureIndex);

  // Get the attribute texture dimensions
  uint width, height, numLevelsUnused;
  attribute.GetDimensions(width, height);

  int y = int(floor(featureIndex / width));
  int x = int(featureIndex - y * width);

  return int3(x, y, 0);
}

// Convert 'float4-encoded uint' to uint
uint _float4ToUint(float4 value)
{
  // RGBA32 texels are provided to the shader as float4 with each byte normalized 0-1
  // Unscale and bit-shift this back to a uint
  uint4 uintValue = uint4(round(255.f * value)) * uint4(1, 0x100, 0x10000, 0x1000000);
  return uintValue.r + uintValue.g + uintValue.b + uintValue.a;
}

// Return the value of the texel at index featureIndex, where addressing is by row starting from 0,0
void ReadFeatureFloatAttribute_float(
  Texture2D attribute,
  float featureIndex,
  out float attributeValue)
{
  attributeValue = attribute.Load(_getAttributeTexelAddress(attribute, featureIndex));
}

// Return the value of the texel at index featureIndex, where addressing is by row starting from 0,0
void ReadFeatureIntegerAttribute_float(
	Texture2D attribute,
	float featureIndex,
	out float valueAsFloat,    // value cast to float (potential loss of precision)
	out float valueMod1000000, // value mod 1,000,000 ("units" component)
	out float valueDiv1000000, // value divided by 1,000,000 (millions component)
	out float2 valueAsFloat2)  // float2 containing {r=valueMod1000000, g=valueDiv1000000}
{
  float4 value = attribute.Load(_getAttributeTexelAddress(attribute, featureIndex));
  int intValue = asint(_float4ToUint(value));

  // set output values
  valueAsFloat = intValue;
  valueMod1000000 = intValue % 1000000;
  valueDiv1000000 = intValue / 1000000;
  valueAsFloat2 = float2(valueMod1000000, valueDiv1000000);
}

// Return the value of the texel at index featureIndex, where addressing is by row starting from 0,0
void ReadFeatureUnsignedIntegerAttribute_float(
	Texture2D attribute,
	float featureIndex,
	out float valueAsFloat,    // value cast to float (potential loss of precision)
	out float valueMod1000000, // value mod 1,000,000 ("units" component)
	out float valueDiv1000000, // value divided by 1,000,000 (millions component)
	out float2 valueAsFloat2)  // float2 containing {r=valueMod1000000, g=valueDiv1000000}
{
  float4 value = attribute.Load(_getAttributeTexelAddress(attribute, featureIndex));
  uint uintValue = _float4ToUint(value);

  // set output values
  valueAsFloat = uintValue;
  valueMod1000000 = uintValue % 1000000;
  valueDiv1000000 = uintValue / 1000000;
  valueAsFloat2 = float2(valueMod1000000, valueDiv1000000);
}

void ApplyColorMode_float(float mode, float4 baseColor, float4 instanceColor, out float3 outColor, out float outAlpha)
{
    if (mode == 0.0)
    {
        outColor = baseColor.xyz;
        outAlpha = baseColor.w;
    }
    else if (mode == 1.0)
    {
        outColor = baseColor.xyz * instanceColor.xyz;
        outAlpha = baseColor.w * instanceColor.w;
    }
    else if (mode == 2.0)
    {
        outColor = dot(baseColor.xyz, float3(0.3, 0.59, 0.11)) * instanceColor.xyz;
        outAlpha = baseColor.w * instanceColor.w;
    }
    else if (mode == 3.0)
    {
        outColor = instanceColor.xyz;
        outAlpha = baseColor.w * instanceColor.w;
    }
}
