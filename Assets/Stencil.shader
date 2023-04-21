Shader "Unlit/Stencil"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        ColorMask 0
        ZWrite off
        
        Stencil
        {
            Ref 1
            Comp always
            Pass replace
            
        }
        
        Pass
        {
        }
    }
}
