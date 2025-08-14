Shader "UI/GaussianBlur"
{
    Properties                   // <- 여기서 _BlurSourceTex 빼버린다
    {
        _MainTex ("Dummy", 2D) = "white" {}
        _Intensity ("Blur Radius", Float) = 2
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Cull Off ZWrite Off Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // 전역 텍스처만 선언
            sampler2D _BlurSourceTex;
            float4    _BlurSourceTex_TexelSize;
            float     _Intensity;

            struct app { float4 vertex:POSITION; float2 uv:TEXCOORD0; };
            struct v2f { float4 pos:SV_POSITION; float2 uv:TEXCOORD0; };

            v2f vert (app v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 texel = _BlurSourceTex_TexelSize.xy * _Intensity;
                float2 uv = i.uv;

                float4 c = 0;
                c += tex2D(_BlurSourceTex, uv + texel * float2(-1,-1)) * 0.0625;
                c += tex2D(_BlurSourceTex, uv + texel * float2( 0,-1)) * 0.125;
                c += tex2D(_BlurSourceTex, uv + texel * float2( 1,-1)) * 0.0625;

                c += tex2D(_BlurSourceTex, uv + texel * float2(-1, 0)) * 0.125;
                c += tex2D(_BlurSourceTex, uv)                        * 0.25;
                c += tex2D(_BlurSourceTex, uv + texel * float2( 1, 0)) * 0.125;

                c += tex2D(_BlurSourceTex, uv + texel * float2(-1, 1)) * 0.0625;
                c += tex2D(_BlurSourceTex, uv + texel * float2( 0, 1)) * 0.125;
                c += tex2D(_BlurSourceTex, uv + texel * float2( 1, 1)) * 0.0625;
                return c;
            }
            ENDHLSL
        }
    }
}
