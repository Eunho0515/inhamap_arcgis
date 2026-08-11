float4 UnpackColor(uint packed)
{
    float inv255 = 1.0f / 255.0f;

    float r = ((packed >> 0) & 0xFF) * inv255;
    float g = ((packed >> 8) & 0xFF) * inv255;
    float b = ((packed >> 16) & 0xFF) * inv255;
    float a = ((packed >> 24) & 0xFF) * inv255;

    return float4(r, g, b, a);
}

float4 UnpackBytesToFloat4(uint packed)
{
    return float4(
        (float)((packed >> 0) & 0xFFu),
        (float)((packed >> 8) & 0xFFu),
        (float)((packed >> 16) & 0xFFu),
        (float)((packed >> 24) & 0xFFu)
    );
}

float4 UnpackUVRegion(uint2 packedRegion)
{
    uint4 uv16 = uint4(
        (packedRegion.x >> 00) & 0xFFFFu,
        (packedRegion.x >> 16) & 0xFFFFu,
        (packedRegion.y >> 00) & 0xFFFFu,
        (packedRegion.y >> 16) & 0xFFFFu
    );

    return float4(uv16) / 65535.0;
}
