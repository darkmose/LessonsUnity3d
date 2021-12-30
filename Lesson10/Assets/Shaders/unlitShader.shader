Shader "Unlit/unlitShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture 1", 2D) = "white" {}
        _SecondTex ("Texture 2", 2D) = "white" {}
        _BlendScale("Blend Scale", Range(0, 1)) = 0.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"


                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                sampler2D _SecondTex;
                half _BlendScale;
                fixed4 _Color;

                v2f vert (appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag (v2f i) : SV_Target
                {
                    fixed4 fragMainTex = tex2D(_MainTex, i.uv);
                    fixed4 fragSecondTex = tex2D(_SecondTex, i.uv);
                    fixed4 finalColor = lerp(fragMainTex, fragSecondTex, _BlendScale);
                    return finalColor * _Color;
                }
                ENDCG
            }
        }
}
