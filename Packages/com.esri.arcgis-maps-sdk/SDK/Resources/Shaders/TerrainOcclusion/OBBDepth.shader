Shader "Unlit/OBBDepth"
{
  SubShader
  {
    Tags { "RenderType" = "Opaque+2" }

    // only draw where 64 was written in the stencil buffer
    Stencil
    {
      ReadMask 64
      Ref 64
      Comp equal
      Pass keep
    }

    Pass
    {
      CGPROGRAM

      #include "UnityCG.cginc"                //commonly used helper functions.
      #include "UnityShaderVariables.cginc" //(automatically included) Commonly used global variables.

      // pragmas
      #pragma vertex vert
      #pragma fragment frag

      // base structs
      struct vertexInput
      {
        float4 vertex: POSITION;
      };

      struct vertexOutput
      {
        float4 pos: SV_POSITION;
      };

      // vertex function
      vertexOutput vert(vertexInput v)
      {
        vertexOutput o;
        o.pos = UnityObjectToClipPos(v.vertex);
        return o;
      }

      // fragment function
      float frag(vertexOutput i) : COLOR
      {
        return i.pos.z;
      }

ENDCG
}
  }
}
