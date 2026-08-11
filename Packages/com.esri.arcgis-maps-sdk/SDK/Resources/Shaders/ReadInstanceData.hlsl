#include "Utils.hlsl"

struct FeatureInstanceData
{
    float4x4 transform;
    uint color;
    float3 scale;
};

StructuredBuffer<FeatureInstanceData> _InstanceBuffer;
StructuredBuffer<uint> _InstanceFlagBuffer;

void ReadInstanceBufferColor_float(float instanceID, out float4 color)
{
    uint packed = _InstanceBuffer[(uint)instanceID].color;

    color = UnpackColor(packed);
}

void ReadColorMode_float(float instanceID, out float colorMode)
{
    static const uint COLOR_MODE_MASK  = 0x6u;

    uint packed = _InstanceFlagBuffer[(uint)instanceID];
    colorMode = (float) ((packed & COLOR_MODE_MASK) >> 1);
}

// Returns the vertex offset in object space so the object is rendered in screen as 1 pixel
// for each world unit. This means that if the object is 100 units height in world space, it
// will be rendered as 100 pixels height in screen no matter the distance to the camera.
void ConstantScreenSizeOffset_float(
    float3 positionWorldSpace,
    float useWorldSpaceUnits,
    out float3 constantScreenSizeOffset)
{
    if (useWorldSpaceUnits > 0.5f)
    {
        constantScreenSizeOffset = float3(0.0f, 0.0f, 0.0f);
        return;
    }

    // The M matrix in an instancing draw call is the world matrix of the instance, not the GameObject.
    float3 pivotWorldSpace = UNITY_MATRIX_M._m03_m13_m23;
    float3 vertexOffsetWorldSpace = positionWorldSpace - pivotWorldSpace;

    float pivotDistanceToCamera = abs(mul(UNITY_MATRIX_V, float4(pivotWorldSpace, 1.0f)).z);
     // World size of 1 pixel at 1 unit depth from the camera
     // This is because the size of a pixel in world space is linearly proportional to depth
    float pixelSizeAtUnitDepth = 2.0f / (UNITY_MATRIX_P._m11 * _ScreenParams.y);
    float screenSpaceScaleFactor = pivotDistanceToCamera * pixelSizeAtUnitDepth;

    // Simplification of worldPosition = pivotWorldSpace + normalizedDirection (no units) *
    // sizeInPixels (pixels) * pixelSizeAtPivotDepth (world units / pixels)
    // where vertexOffsetWorldSpace is normalizedDirection * sizeInPixels because we want to
    // render as many pixels as world units height the object is
    float3 scaledPositionWorldSpace = pivotWorldSpace + vertexOffsetWorldSpace * screenSpaceScaleFactor;
    float3 offsetWorldSpace = scaledPositionWorldSpace - positionWorldSpace;

    constantScreenSizeOffset = mul(UNITY_MATRIX_I_M, float4(offsetWorldSpace, 1.0f)).xyz;
}
