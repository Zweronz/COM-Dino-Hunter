Shader "Triniti/Particle/AA_COL_DO_P10" {
Properties {
 _Color ("Main Color", Color) = (0.5,0.5,0.5,0.5)
 _MainTex ("Particle Texture", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent+10" }
 Pass {
  Tags { "QUEUE"="Transparent+10" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "color", Color
   Bind "texcoord", TexCoord
  }
  ZWrite Off
  Cull Off
  Fog {
   Color (0,0,0,0)
  }
  Blend SrcAlpha One
  SetTexture [_MainTex] { ConstantColor [_Color] combine constant * primary }
  SetTexture [_MainTex] { combine texture * previous double }
 }
}
}