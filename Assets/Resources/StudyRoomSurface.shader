Shader "StudyRoom/Surface"
{
    Properties { _Color ("Color", Color) = (1,1,1,1) _MainTex ("Picture", 2D) = "white" {} }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            struct appdata { float4 vertex : POSITION; float3 normal : NORMAL; float2 uv : TEXCOORD0; };
            struct v2f { float4 position : SV_POSITION; float shade : TEXCOORD0; float2 uv : TEXCOORD1; };
            fixed4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.shade = .65 + .35 * saturate(dot(UnityObjectToWorldNormal(v.normal), normalize(float3(-.4,1,-.6))));
                return o;
            }
            fixed4 frag(v2f i) : SV_Target { return tex2D(_MainTex, i.uv) * fixed4(_Color.rgb * i.shade, _Color.a); }
            ENDCG
        }
    }
}
