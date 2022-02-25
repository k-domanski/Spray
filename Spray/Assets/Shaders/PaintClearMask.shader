Shader "Unlit/PaintMask"
{
	Properties
	{
		_ColorTex("Texture", 2D) = "white" {}
		_NormalTex("Texture", 2D) = "white" {}
		_BrushTex("Brush Pattern", 2D) = "white" {}
		_BrushNormalTex("Brush Normal Texture", 2D) = "bump" {}
		_Color("Color", Color) = (1, 0, 0, 1)
	}

		SubShader
		{
			Cull Off ZWrite Off ZTest Off
			Blend Off

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
					float3 bitangent : TEXCOORD3;
				};

				sampler2D _ColorTex;
				float4 _ColorTex_ST;

				sampler2D _NormalTex;
				float4 _NormalTex_ST;

				sampler2D _BrushTex;
				float4 _BrushTex_ST;

				bool _RandomNormal = true;
				float _NormalStrength;

				sampler2D _BrushNormalTex;
				float4 _BrushNormalTex_ST;

				float _Radius;
				float _Hardness;
				float _Strength;

				float3 _BrushPos;
				float _BrushRotation;

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

				v2f vert(appdata IN)
				{
					v2f OUT;
					OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex);
					OUT.uv = IN.uv;
					OUT.normal = UnityObjectToWorldNormal(IN.normal);
					OUT.tangent = UnityObjectToWorldDir(IN.tangent.xyz);
					//OUT.bitangent = cross(OUT.normal, OUT.tangent) * IN.tangent.w * unity_WorldTransformParams.w;
					OUT.bitangent = cross(OUT.normal, OUT.tangent) * IN.tangent.w;

					float4 position = float4(0, 0, 0, 1);
					position.xy = (IN.uv.xy * float2(2, 2) - float2(1, 1)) * float2(1, _ProjectionParams.x);
					OUT.vertex = position;
					return OUT;
				}

				struct RenderData
				{
					float4 Color : SV_TARGET0;
					float4 Normal : SV_TARGET1;
				};

				RenderData frag(v2f IN) : SV_Target
				{
					RenderData render_data;

					float3 normal = normalize(IN.normal);
					float3 tangent = normalize(IN.tangent);
					float3 bitangent = normalize(IN.bitangent);

					float3x3 tangent_mat = float3x3(tangent, bitangent, normal);

					/* Sample brush for alpha */
					float3 delta = IN.worldPos - _BrushPos;
					float3 p = clamp(delta / _Radius, -1, 1);
					float2 coeffs = float2(dot(p, tangent), dot(p, bitangent));
					float2 uv = rotateUV(coeffs * 0.5 + 0.5, _BrushRotation);
					float4 brush = tex2D(_BrushTex, uv);

					/* Mask - radius based on brush position */
					float m = mask(_BrushPos, IN.worldPos, _Radius, _Hardness);
					float mask = m * brush.a * _Strength;

					/* Color Clear */
					float4 color = tex2D(_ColorTex, IN.uv);
					//float target_alpha = lerp(color.a, 0, _Strength);
					//color.a = lerp(color.a, target_alpha, mask);
					float target_alpha = min(color.a, 0.08);
					color.a = lerp(color.a, target_alpha, mask);
					render_data.Color = color;

					/* Normal Clear */
					float4 tex_normal = tex2D(_NormalTex, IN.uv);
					render_data.Normal = lerp(tex_normal, float4(normal, 0), mask);

					return render_data;
				}
				ENDCG
			}
		}
}
