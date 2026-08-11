Shader "Unlit/StencilWrite"
{
  SubShader
  {
    Tags { "RenderType" = "Opaque+1" }

    LOD 100

    // don't render anything
    ColorMask 0

    // don't write to the depth buffer
    ZWrite On

    // Some layers may have triangles with mixed winding orders.
    Cull Off

    // write 64 to the stencil buffer
    Stencil
    {
      WriteMask 64
      Ref 255
      Comp always
      Pass replace
    }

    Pass
    {
      CGPROGRAM

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
        return 0;
      }

      ENDCG
    }
        
  }
}
