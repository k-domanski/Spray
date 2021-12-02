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

				float4 _Color;
				float _Radius;
				float _Hardness;
				float _Strength;

				float3 _BrushPos;
				float _BrushRotation;

				/* Gradient Noise */
				float2 unity_gradientNoise_dir(float2 p)
				{
					p = p % 289;
					float x = (34 * p.x + 1) * p.x % 289 + p.y;
					x = (34 * x + 1) * x % 289;
					x = frac(x / 41) * 2 - 1;
					return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
				}

				float unity_gradientNoise(float2 p)
				{
					float2 ip = floor(p);
					float2 fp = frac(p);
					float d00 = dot(unity_gradientNoise_dir(ip), fp);
					float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
					float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
					float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
					fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
					return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
				}

				float GradientNoise(float2 UV, float Scale)
				{
					return unity_gradientNoise(UV * Scale) + 0.5;
				}
				/* Gradient Noise End */

				/* Simple Noise */
				inline float unity_noise_randomValue(float2 uv)
				{
					return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453 * _Time.y);
				}

				inline float unity_noise_interpolate(float a, float b, float t)
				{   /* Can be replaced with Lerp? */
					return (1.0 - t) * a + (t * b);
				}

				inline float unity_valueNoise(float2 uv)
				{
					float2 i = floor(uv);
					float2 f = frac(uv);
					f = f * f * (3.0 - 2.0 * f);

					uv = abs(frac(uv) - 0.5);
					float2 c0 = i + float2(0.0, 0.0);
					float2 c1 = i + float2(1.0, 0.0);
					float2 c2 = i + float2(0.0, 1.0);
					float2 c3 = i + float2(1.0, 1.0);
					float r0 = unity_noise_randomValue(c0);
					float r1 = unity_noise_randomValue(c1);
					float r2 = unity_noise_randomValue(c2);
					float r3 = unity_noise_randomValue(c3);

					float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
					float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
					float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
					return t;
				}

				float SimpleNoise(float2 UV, float Scale)
				{
					float t = 0.0;

					float freq = pow(2.0, float(0));
					float amp = pow(0.5, float(3 - 0));
					t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

					freq = pow(2.0, float(1));
					amp = pow(0.5, float(3 - 1));
					t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

					freq = pow(2.0, float(2));
					amp = pow(0.5, float(3 - 2));
					t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

					return t;
				}
				/* Simple Noise End */

				/* Voronoi */
				inline float2 unity_voronoi_noise_randomVector(float2 UV, float offset)
				{
					float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
					UV = frac(sin(mul(UV, m)) * 46839.32);
					return float2(sin(UV.y * +offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
				}

				float2 Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity)
				{
					float2 g = floor(UV * CellDensity);
					float2 f = frac(UV * CellDensity);
					float t = 8.0;
					float3 res = float3(8.0, 0.0, 0.0);

					for (int y = -1; y <= 1; y++)
					{
						for (int x = -1; x <= 1; x++)
						{
							float2 lattice = float2(x,y);
							float2 offset = unity_voronoi_noise_randomVector(lattice + g, AngleOffset);
							float d = distance(lattice + offset, f);
							if (d < res.x)
							{
								res = float3(d, offset.x, offset.y);
							}
						}
					}

					return res.xy;
				}
				/* Voronoi End*/

				float3 NormalFromHeight(float In, float3 WorldPos, float3x3 TangentMatrix, float Strength)
				{
					float3 worldDerivativeX = ddx(WorldPos);
					float3 worldDerivativeY = ddy(WorldPos);

					float3 crossX = cross(TangentMatrix[2].xyz, worldDerivativeX);
					float3 crossY = cross(worldDerivativeY, TangentMatrix[2].xyz);
					float d = dot(worldDerivativeX, crossY);
					float sgn = d < 0.0 ? (-1.0f) : 1.0f;
					float surface = sgn / max(0.000000000000001192093f, abs(d));

					float dHdx = ddx(In);
					float dHdy = ddy(In);
					float3 surfGrad = surface * (dHdx * crossY + dHdy * crossX);
					float3 Out = normalize(TangentMatrix[2].xyz - (Strength * surfGrad));
					return mul(transpose(TangentMatrix), Out);
				}

				float3 PackNormal(float3 normal)
				{
					return normal * 0.5 + 0.5;
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

				v2f vert(appdata IN)
				{
					v2f OUT;
					OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex);
					OUT.uv = IN.uv;
					OUT.normal = UnityObjectToWorldNormal(IN.normal);
					OUT.tangent = UnityObjectToWorldDir(IN.tangent.xyz);
					OUT.bitangent = cross(OUT.normal, OUT.tangent) * IN.tangent.w * unity_WorldTransformParams.w;

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

					/* Normal */
					half3 brushNormal = UnpackNormal(tex2D(_BrushNormalTex, uv));

					/* Mask - radius based on brush position */
					float m = mask(_BrushPos, IN.worldPos, _Radius, _Hardness);
					float mask = m * brush.a * _Strength;

					/* Swap texture color */
					float4 color = tex2D(_ColorTex, IN.uv);
					render_data.Color = lerp(color, _Color, mask);

					float4 tex_normal = tex2D(_NormalTex, IN.uv);
					float height = SimpleNoise(GradientNoise(uv, 5.0), 5.0) + 0.0;
					float3 texture_normal = NormalFromHeight(height, IN.worldPos, tangent_mat, _NormalStrength) * 0.5 + 0.5;

					if (_RandomNormal == true)
					{
						render_data.Normal = lerp(tex_normal, float4(texture_normal, 1), mask);
					}
					else
					{
						render_data.Normal = lerp(tex_normal, float4(brushNormal.xyz, 1), mask);
					}

					return render_data;
				}
				ENDCG
			}
		}
}
