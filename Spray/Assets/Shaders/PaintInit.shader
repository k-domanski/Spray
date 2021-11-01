Shader "Unlit/PaintMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            HLSLPROGRAM
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
                float4 worldPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _BrushPos;

            v2f vert (appdata IN)
            {
                v2f OUT;
                OUT.worldPosition = mul(unity_ObjectToWorld, IN.vertex);
                OUT.uv = IN.uv;

                float4 position = float4(0.0f, 0.0f, 0.0f, 1.0f);
                position.xy = (IN.uv.xy * 2.0f - 1.0f) * float2(1.0f, _ProjectionParams.x);
                OUT.vertex = position;
                return OUT;
            }

            float mask(float4 brushPos, float4 worldPos, float radius, float hardness)
            {
                float dist = distance(brushPos, worldPos);
                return 1.0f - smoothstep(radius * hardness, radius, dist);
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                return float4(1.0f, 0.0f, 0.0f, 1.0f);
            }
            ENDHLSL
        }
    }
}
