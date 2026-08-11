Shader "Custom/GaussianSplatAccumulate"
{
    Properties
    {
        [HideInInspector] _GaussianSplatAtlas1 ("Gaussian Splat Atlas 1", 2D) = "black" {}
        [HideInInspector] _GaussianSplatAtlas2 ("Gaussian Splat Atlas 2", 2D) = "black" {}
        [HideInInspector] _GaussianSplatAtlas3 ("Gaussian Splat Atlas 3", 2D) = "black" {}
        [HideInInspector] _GaussianSplatAtlas4 ("Gaussian Splat Atlas 4", 2D) = "black" {}
        [HideInInspector] _GaussianSplatAtlas5 ("Gaussian Splat Atlas 5", 2D) = "black" {}
        [HideInInspector] _GaussianSplatAtlas6 ("Gaussian Splat Atlas 6", 2D) = "black" {}
        [HideInInspector] _GaussianSplatAtlas7 ("Gaussian Splat Atlas 7", 2D) = "black" {}
        [HideInInspector] _GaussianSplatAtlas8 ("Gaussian Splat Atlas 8", 2D) = "black" {}
        [HideInInspector] _ClippingMode ("Clipping Mode", Int) = 0
        [HideInInspector] _MapAreaMin ("Map Area Min", Vector) = (0, 0, 0, 0)
        [HideInInspector] _MapAreaMax ("Map Area Max", Vector) = (0, 0, 0, 0)
    }

	HLSLINCLUDE
    #include "UnityCG.cginc"
    #include "../AlphaClipping.hlsl"

    static const float INV_SQRT_TWO = 0.707106781186547524401;
    static const float QUANTIZATION_SCALE = 2.048;

    struct appdata
    {
        float4 vertex : POSITION;
        uint instanceID : SV_InstanceID;
    };

    struct VS_output
    {
        float4 position : SV_POSITION;
        float2 uv : TEXCOORD0;
        float3 worldPosition : TEXCOORD1;
        float depth : TEXCOORD2;
        float4 color: COLOR;
    };

    struct FS_Output
    {
        float4 color : SV_Target0;
        float4 depthAlpha : SV_Target1;
    };

    StructuredBuffer<uint> _InstanceBuffer;
    Texture2D<float4> _GaussianSplatAtlas1;
    Texture2D<float4> _GaussianSplatAtlas2;
    Texture2D<float4> _GaussianSplatAtlas3;
    Texture2D<float4> _GaussianSplatAtlas4;
    Texture2D<float4> _GaussianSplatAtlas5;
    Texture2D<float4> _GaussianSplatAtlas6;
    Texture2D<float4> _GaussianSplatAtlas7;
    Texture2D<float4> _GaussianSplatAtlas8;
    float4x4 _LocalToWorld;
    float4x4 _WorldToLocal;
    float3 _WorldCameraPos;
    float3 _CameraPositionQuantized;
    float3 _CameraPositionDelta;
    float4 _WorldRotation;
    int _ClippingMode;
    float3 _MapAreaMin;
    float3 _MapAreaMax;

    bool getPackedSplatAndHeader(uint splatId, out uint4 packedSplat, out uint4 packedHeader)
    {
        uint width;
        uint height;
        packedSplat = 0;
        packedHeader = 0;
        _GaussianSplatAtlas1.GetDimensions(width, height);

        if (width == 0u || height == 0u)
        {
            return false;
        }

        const uint texelsPerPage = 1024u;
        const uint splatsPerPage = texelsPerPage - 1u;

        const uint texelsPerAtlas = width * height;
        const uint atlasTextureIndex = splatId / texelsPerAtlas;

        const uint splatTexelIndex = splatId % texelsPerAtlas;
        const uint splatHeaderIndex = (splatTexelIndex & 0xFFFFFC00u) | splatsPerPage;

        int2 splatTexelCoord = int2(splatTexelIndex % width, splatTexelIndex / width);
        int2 headerTexelCoord = int2(splatHeaderIndex % width, splatHeaderIndex / width);

        switch (atlasTextureIndex)
        {
            case 0u:
                packedSplat = asuint(_GaussianSplatAtlas1.Load(int3(splatTexelCoord, 0)));
                packedHeader = asuint(_GaussianSplatAtlas1.Load(int3(headerTexelCoord, 0)));
                break;
            case 1u:
                packedSplat = asuint(_GaussianSplatAtlas2.Load(int3(splatTexelCoord, 0)));
                packedHeader = asuint(_GaussianSplatAtlas2.Load(int3(headerTexelCoord, 0)));
                break;
            case 2u:
                packedSplat = asuint(_GaussianSplatAtlas3.Load(int3(splatTexelCoord, 0)));
                packedHeader = asuint(_GaussianSplatAtlas3.Load(int3(headerTexelCoord, 0)));
                break;
            case 3u:
                packedSplat = asuint(_GaussianSplatAtlas4.Load(int3(splatTexelCoord, 0)));
                packedHeader = asuint(_GaussianSplatAtlas4.Load(int3(headerTexelCoord, 0)));
                break;
            case 4u:
                packedSplat = asuint(_GaussianSplatAtlas5.Load(int3(splatTexelCoord, 0)));
                packedHeader = asuint(_GaussianSplatAtlas5.Load(int3(headerTexelCoord, 0)));
                break;
            case 5u:
                packedSplat = asuint(_GaussianSplatAtlas6.Load(int3(splatTexelCoord, 0)));
                packedHeader = asuint(_GaussianSplatAtlas6.Load(int3(headerTexelCoord, 0)));
                break;
            case 6u:
                packedSplat = asuint(_GaussianSplatAtlas7.Load(int3(splatTexelCoord, 0)));
                packedHeader = asuint(_GaussianSplatAtlas7.Load(int3(headerTexelCoord, 0)));
                break;
            case 7u:
                packedSplat = asuint(_GaussianSplatAtlas8.Load(int3(splatTexelCoord, 0)));
                packedHeader = asuint(_GaussianSplatAtlas8.Load(int3(headerTexelCoord, 0)));
                break;
            default:
                return false;
        }

        return true;
    }

    float3 unpackPosition(uint4 packed, float invPosScale)
    {
        float3 pos;
        pos.x = (float)(packed.y & 16383u);
        pos.y = (float)((packed.y >> 14u) & 16383u);
        pos.z = (float)((packed.y >> 28u) | ((packed.z & 1023u) << 4u));
        return pos * invPosScale;
    }

    float3 unpackScale(uint4 packed)
    {
        float3 scale;
        scale.x = (float)((packed.z >> 10u) & 0xffu);
        scale.y = (float)((packed.z >> 18u) & 0xffu);
        scale.z = (float)(((packed.z >> 26u) & 63u) | ((packed.w & 3u) << 6u));
        return exp(scale / 16.0 - 10.0);
    }

    float4 unpackRotation(uint4 packed)
    {
        uint comp = packed.x;
        float q[4];
        const uint cMask = (1u << 9u) - 1u;
        const uint largestIndex = comp >> 30u;
        float sumSquares = 0.0;

        int3 componentMap[4];
        componentMap[0] = int3(3, 2, 1);
        componentMap[1] = int3(3, 2, 0);
        componentMap[2] = int3(3, 1, 0);
        componentMap[3] = int3(2, 1, 0);

        int3 remainingIndices = componentMap[largestIndex];

        [unroll]
        for (uint componentIndex = 0u; componentIndex < 3u; ++componentIndex)
        {
            uint outputIndex = (uint)remainingIndices[componentIndex];
            uint magnitude = comp & cMask;
            uint signBit = (comp >> 9u) & 0x1u;
            comp >>= 10u;

            q[outputIndex] = INV_SQRT_TWO * (float)magnitude / (float)cMask;
            if (signBit == 1u)
            {
                q[outputIndex] = -q[outputIndex];
            }

            sumSquares += q[outputIndex] * q[outputIndex];
        }

        q[largestIndex] = sqrt(max(1.0 - sumSquares, 0.0));

        return float4(q[0], q[1], q[2], q[3]);
    }

    // Decode color from the packed texel. Input is expected to be in Gamma space. Output is in the color space of Unity project.
    float4 unpackColor(uint4 packed)
    {
        float4 color;
        color.r = (float)((packed.w >> 1u) & 254u) / 255.0;
        color.g = (float)((packed.w >> 9u) & 255u) / 255.0;
        color.b = (float)((packed.w >> 16u) & 254u) / 255.0;
        color.a = (float)((packed.w >> 24u) & 255u) / 255.0;
#if !defined(UNITY_COLORSPACE_GAMMA)
        color.rgb = GammaToLinearSpace(color.rgb);
#endif
        return color;
    }

    // Reconstruct a position from a quantized position and a delta within the cell grid
    float3 dequantizePosition(float3 quantizedValue, float3 delta)
    {
        return quantizedValue * QUANTIZATION_SCALE + delta;
    }

    // Reconstruct a splat center as a camera-relative offset using the packed local position
    float4 getPositionRelativeToCamera(uint4 packed, uint4 hdr, float3 quantizedCamPos, float3 camDelta)
    {
        float3 quantizedSourcePos = float3(asfloat(hdr.x), asfloat(hdr.y), asfloat(hdr.z));

        uint hdrCode = hdr.w;
        float invPosScale = 1.0 / (float)(1u << (hdrCode & 255u));
        float3 splatPos = unpackPosition(packed, invPosScale);

        // Restructuring this to `dequantizePosition(quantizedSourcePos, splatPos) - dequantizePosition(quantizedCamPos, camDelta)` causes rounding errors
        splatPos = dequantizePosition(quantizedSourcePos - quantizedCamPos, splatPos - camDelta);

        return float4(splatPos, 1.0);
    }

    // Calculate the rotation matrix associated with a normalized quaternion.
    float3x3 getRotationMatrix(float4 q)
    {
        float x2 = q.x + q.x;
        float y2 = q.y + q.y;
        float z2 = q.z + q.z;

        float xx = x2 * q.x;
        float xy = x2 * q.y;
        float xz = x2 * q.z;
        float xw = x2 * q.w;
        float yy = y2 * q.y;
        float yz = y2 * q.z;
        float yw = y2 * q.w;
        float zz = z2 * q.z;
        float zw = z2 * q.w;

        return float3x3(
            float3(1.0 - (yy + zz), xy + zw, xz - yw),
            float3(xy - zw, 1.0 - (xx + zz), yz + xw),
            float3(xz + yw, yz - xw, 1.0 - (xx + yy))
        );
    }

    float3x3 diagonalMatrix(float3 diagonal)
    {
        return float3x3(
            diagonal.x, 0.0, 0.0,
            0.0, diagonal.y, 0.0,
            0.0, 0.0, diagonal.z
        );
    }

    float3 getTransformBasisLengths(float4x4 transformMatrix)
    {
        const float3 basisX = float3(transformMatrix._11, transformMatrix._21, transformMatrix._31);
        const float3 basisY = float3(transformMatrix._12, transformMatrix._22, transformMatrix._32);
        const float3 basisZ = float3(transformMatrix._13, transformMatrix._23, transformMatrix._33);

        return float3(length(basisX), length(basisY), length(basisZ));
    }

    bool anyComponentGreaterThan(float2 a, float2 b)
    {
        return a.x > b.x || a.y > b.y;
    }

    // Project a 3D splat to viewport and return the center and scaled major and minor axes of the billboard
    bool projectSplat(
        float4x4 viewMat,
        float4x4 projMat,
        float2 viewport,
        float4 centerVs,
        float3 scale,
        float4 quaternion,
        out float4 projectedCenter,
        out float2 axis1,
        out float2 axis2)
    {
        axis1 = 0;
        axis2 = 0;
        projectedCenter = float4(0, 0, 0, 0);

        // discard if splat is behind the camera
        if (centerVs.z > 0.0)
        {
            return false;
        }

        projectedCenter = mul(projMat, centerVs);

        // clamp to camera near/far planes
        projectedCenter.z = clamp(projectedCenter.z, -abs(projectedCenter.w), abs(projectedCenter.w));
        centerVs.xyz /= centerVs.w;

        // Given a rotation matrix and scale vector, compute 3d covariance A and B
        float3x3 rotation = getRotationMatrix(normalize(quaternion));
        const float3 decodedBasisLengths = getTransformBasisLengths(_LocalToWorld);
        const float3 covarianceScale = scale * decodedBasisLengths;
        float3x3 scaleMatrix = diagonalMatrix(covarianceScale.xzy);
        float3x3 transformMatrix = mul(scaleMatrix, rotation);
        float3x3 covariance3d = mul(transpose(transformMatrix), transformMatrix);

        // Calculate the projection and extents based on https://www.sctheblog.com/blog/gaussian-splatting
        float3 viewPos = centerVs.xyz;
        float2 cameraFocalLength = float2(viewport.x * projMat._11, viewport.y * projMat._22);
        float2 jacobianScale = cameraFocalLength / viewPos.z;
        float2 jacobianOffset = -jacobianScale / viewPos.z * viewPos.xy;
        float3x3 jacobian = transpose(float3x3(
            jacobianScale.x, 0.0, jacobianOffset.x,
            0.0, jacobianScale.y, jacobianOffset.y,
		            0.0, 0.0, 0.0
        ));
        float3x3 viewRotation = transpose((float3x3) viewMat);
        float3x3 projectedTransform = mul(viewRotation, jacobian);
        float3x3 covariance2d = mul(transpose(projectedTransform), mul(covariance3d, projectedTransform));

        float diagonal1 = covariance2d[0][0] + 0.3;
        float offDiagonal = covariance2d[0][1];
        float diagonal2 = covariance2d[1][1] + 0.3;

        float midpoint = 0.5 * (diagonal1 + diagonal2);
        float eigenRadius = length(float2(0.5 * (diagonal1 - diagonal2), offDiagonal));
        float lambda1 = midpoint + eigenRadius;
        float lambda2 = max(midpoint - eigenRadius, 0.1);

        float extent1 = 3.0 * min(sqrt(lambda1), 1024.0);
        float extent2 = 3.0 * min(sqrt(lambda2), 1024.0);

        // discard if the projected splat is smaller than 2 pixels in both dimensions
        if (extent1 < 2.0 && extent2 < 2.0)
        {
            return false;
        }

        // discard if the projected splat is outside the viewport
        if (anyComponentGreaterThan(abs(projectedCenter.xy) - float2(extent1, extent2) / viewport * projectedCenter.w, projectedCenter.ww))
        {
            return false;
        }

        // calculate the major and minor axes of the splat
        float2 majorAxis = normalize(float2(offDiagonal, lambda1 - diagonal1));
        axis1 = extent1 * majorAxis;
        axis2 = extent2 * float2(majorAxis.y, -majorAxis.x);

        return true;
    }

    // Multiplies and normalizes two quaternions (q2 * q1 Hamilton product, first applies q1 rotation then q2)
    float4 CombineRotations(float4 q1, float4 q2)
    {
        float4 q;

        q.w = q2.w * q1.w - q2.x * q1.x - q2.y * q1.y - q2.z * q1.z;
        q.x = q2.w * q1.x + q2.x * q1.w + q2.y * q1.z - q2.z * q1.y;
        q.y = q2.w * q1.y - q2.x * q1.z + q2.y * q1.w + q2.z * q1.x;
        q.z = q2.w * q1.z + q2.x * q1.y - q2.y * q1.x + q2.z * q1.w;

        return normalize(q);
    }

    VS_output vert(appdata v)
    {
        VS_output o;
        o.position = float4(0.0, 0.0, 0.0, -1.0);
        o.worldPosition = 0.0;
        o.uv = 0.0;
        o.color = 0.0;
        o.depth = 0.0;

        // Extract the packed splat data and header from the texture
        uint splatId = _InstanceBuffer[v.instanceID];
        uint4 packedSplat;
        uint4 packedHeader;
        if (!getPackedSplatAndHeader(splatId, packedSplat, packedHeader))
        {
            return o;
        }

        // Unpack the splat data from the raw uint4
        float3 scale = unpackScale(packedSplat);
        float4 quaternion = unpackRotation(packedSplat);
        float4 color = unpackColor(packedSplat);

        // Apply the inverse of HPT root rotation to the splat's quaternion
        quaternion = CombineRotations(quaternion, _WorldRotation);

        // Reconstruct the splat center in the view space using its position relative to camera
        float4 centerCameraRelative = getPositionRelativeToCamera(packedSplat, packedHeader, _CameraPositionQuantized , _CameraPositionDelta);
        float3 centerWS = _WorldCameraPos + mul(_LocalToWorld, float4(centerCameraRelative.xyz, 0.0)).xyz ;

        float clippingAlpha = 1.0;
        AlphaClipping_float(centerWS, _WorldToLocal, _ClippingMode, _MapAreaMin, _MapAreaMax, clippingAlpha);
        if (clippingAlpha < 0.5)
        {
            return o;
        }

        float4 centerVS = float4(mul(UNITY_MATRIX_V, float4(centerWS, 1.0)).xyz, 1.0);

        float2 axis1;
        float2 axis2;
        float4 centerCS;
        bool isVisible = projectSplat(UNITY_MATRIX_V, UNITY_MATRIX_P, _ScreenParams.xy, centerVS, scale, quaternion, centerCS, axis1, axis2);
        if (!isVisible || centerCS.w <= 0.0)
        {
            return o;
        }

        // Calculate the clip-space position of the current vertex
        float2 pixelOffset = (2.0 / _ScreenParams.xy) * (axis1 * v.vertex.x + axis2 * v.vertex.y) * centerCS.w;
        o.position = centerCS + float4(pixelOffset, 0, 0);
        o.worldPosition = centerWS;
        o.depth = min(centerCS.z / centerCS.w, 1.0);
        o.color = color;
        o.uv = v.vertex.xy * 2.0;

        return o;
    }

    FS_Output frag(VS_output i)
    {
        FS_Output o;
        float alpha = 1;

        float radius = dot(i.uv, i.uv);
        if (radius > 1.0)
        {
            discard;
        }

        alpha = exp(-radius * 4.0) * i.color.a;
        if (alpha < (1.0 / 255.0))
        {
            discard;
        }

        o.color = float4(i.color.rgb * alpha, alpha);
        o.depthAlpha = float4(i.depth, 0.0, 0.0, alpha);
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
            Name "GaussianSplatAccumulate"
            Tags { "LightMode"="UniversalForward" }
            Blend 0 OneMinusDstAlpha One
            Blend 1 OneMinusDstAlpha DstAlpha
            ZWrite Off
            ZTest Always

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex vert
            #pragma fragment frag
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
            Name "GaussianSplatAccumulate"
            Tags { "LightMode"="ForwardOnly" }
            Blend 0 OneMinusDstAlpha One
            Blend 1 OneMinusDstAlpha DstAlpha
            ZWrite Off
            ZTest Always

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex vert
            #pragma fragment frag
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
            Name "GaussianSplatAccumulate"
            Blend 0 OneMinusDstAlpha One
            Blend 1 OneMinusDstAlpha DstAlpha
            ZWrite Off
            ZTest Always

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
        }
    }
}
