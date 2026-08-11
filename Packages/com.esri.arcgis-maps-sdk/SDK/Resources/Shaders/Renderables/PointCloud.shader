Shader "Custom/PointCloud"
{
    Properties
    {
        [HideInInspector] _PointCloudPointSize ("Point Cloud Point Size", Float) = 4
        [HideInInspector] _UseWorldSpaceUnits ("Use World Space Units", Float) = 0
        [HideInInspector] _ClippingMode ("Clipping Mode", Int) = 0
        [HideInInspector] _MapAreaMin ("Map Area Min", Vector) = (0, 0, 0, 0)
        [HideInInspector] _MapAreaMax ("Map Area Max", Vector) = (0, 0, 0, 0)
        [HideInInspector] _Opacity ("Opacity", Float) = 1
    }

    HLSLINCLUDE
    #if defined(UNITY_RENDER_PIPELINE_UNIVERSAL)
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #elif defined(UNITY_RENDER_PIPELINE_HIGH_DEFINITION) || defined(UNITY_RENDER_PIPELINE_HDRP)
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
    #else
        #include "UnityCG.cginc"
    #endif
    #define UNITY_INDIRECT_DRAW_ARGS IndirectDrawIndexedArgs
    #include "UnityIndirect.cginc"
    #include "../AlphaClipping.hlsl"
    #include "../Billboard.hlsl"
    #include "../Utils.hlsl"

    struct appdata
    {
        // vertex defines the coordinates of a unit square, and its purpose is to specify
        // the relative offset of each vertex within the billboard. It needs to work together
        // with the billboard's size, since the actual billboard size depends on whether this
        // offset calculation is done in local space or in clip space.
        float4 vertex : POSITION;
        uint instanceID : SV_InstanceID;
    };

    struct VS_output
    {
        float4 position : SV_POSITION;
        float2 uv : TEXCOORD0;
        float3 worldPosition : TEXCOORD1;
        float4 color : COLOR;
    };

    struct InstanceData
    {
        float3 relativePosition;
        uint rgba;
    };
    StructuredBuffer<InstanceData> _InstanceBuffer;

    float4x4 _LocalToWorld;
    float4x4 _WorldToLocal;
    float _PointCloudPointSize;
    float _UseWorldSpaceUnits;
    int _ClippingMode;
    float3 _MapAreaMin;
    float3 _MapAreaMax;
    float _Opacity;

    VS_output vert(appdata v)
    {
        InitIndirectDrawArgs(0);

        VS_output o;

        InstanceData instanceData = _InstanceBuffer[v.instanceID];

        float4 centerWorld = mul(_LocalToWorld, float4(instanceData.relativePosition, 1.0));
        float4 fillColor = UnpackColor(instanceData.rgba);

        o.position = CalculateBillboardClipPosition(
            _UseWorldSpaceUnits > 0.5,
            v.vertex.xy,
            float2(_PointCloudPointSize, _PointCloudPointSize),
            centerWorld,
            true);

        o.worldPosition = centerWorld.xyz;
        o.uv = float2(v.vertex.x, v.vertex.y) * 2.0;
        o.color = float4(fillColor.rgb, fillColor.a * _Opacity);

        return o;
    }

    fixed4 frag(VS_output i) : SV_Target
    {
        float clippingAlpha = 1.0;
        AlphaClipping_float(i.worldPosition, _WorldToLocal, _ClippingMode, _MapAreaMin, _MapAreaMax, clippingAlpha);
        clip(clippingAlpha - 0.5);

        float radius = length(i.uv);
        clip(1 - radius);

        return i.color;
    }
    ENDHLSL

    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        LOD 100
        Cull Off

        Pass
        {
            Tags { "LightMode"="UniversalForward" }
    	    Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
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
            Tags { "LightMode"="ForwardOnly" }
    	    Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
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
    	    Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            ENDHLSL
        }
    }
}
