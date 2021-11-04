Shader "Unlit/PaintMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BrushTex ("Brush Pattern", 2D) = "white" {}
        _Color("Color", Color) = (1, 0, 0, 1)

    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Off

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
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 normal : NORMAL;
                float3 tangent : TEXCOORD2;
                float3 binormal : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _BrushTex;
            float4 _BrushTex_ST;

            float4 _Color;
            float _Radius;
            float _Hardness;
            float _Strength;

            float3 _BrushPos;
            float _BrushRotation;

            v2f vert (appdata IN)
            {
                v2f OUT;
                OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex);
                OUT.uv = IN.uv;
                OUT.normal = UnityObjectToWorldNormal(IN.normal);
                OUT.tangent = UnityObjectToWorldDir(IN.tangent.xyz);
                OUT.binormal = cross(OUT.normal, OUT.tangent) * IN.tangent.w * unity_WorldTransformParams.w;

                float4 position = float4(0, 0, 0, 1);
                position.xy = (IN.uv.xy * float2(2, 2) - float2(1, 1)) * float2(1, _ProjectionParams.x);
                OUT.vertex = position;
                return OUT;
            }

            float mask(float3 brushPos, float3 worldPos, float radius, float hardness)
            {
                float dist = distance(brushPos.xyz, worldPos.xyz);
                return 1.0f - smoothstep(radius * hardness, radius, dist);
            }

            float2 rotateUV(float2 uv, float angle)
            {
                float s = sin(angle);
                float c = cos(angle);
                return mul(float2x2(c, -s, s, c), uv - 0.5) + 0.5;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                float3 normal = normalize(IN.normal);
                float3 tangent = normalize(IN.tangent);
                float3 binormal = normalize(IN.binormal);

                /* Swap texture color */
                float4 color = tex2D(_MainTex, IN.uv);

                /* Sample brush for alpha */
                float3 delta = IN.worldPos - _BrushPos;
                float3 p = clamp(delta/_Radius, -1, 1);
                float2 coeffs = float2(dot(p, tangent), dot(p, binormal));
                float2 uv = rotateUV(coeffs * 0.5 + 0.5, _BrushRotation);
                float4 brush = tex2D(_BrushTex, uv);
                
                /* Mask - radius based on brush position */
                float m = mask(_BrushPos, IN.worldPos, _Radius, _Hardness);
                return lerp(color, _Color, brush.a * m * _Strength);


            }
            ENDCG
        }
    }
}
