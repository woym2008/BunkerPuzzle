Shader "Sprites/Grid_1"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_P1("P1", Range(0,10)) = 2
		_P2("P2", Range(0,10)) = 2
		_P3("P3", Range(0,10)) = 1
		_P4("P4", Range(0,5)) = 1
		_P5("P5", Range(0,1)) = .5
		_P6("P6", Range(0,1)) = .5



		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One One

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma shader_feature _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = tex2D (_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}

			float _P1;
			float _P2;
			float _P3;
			float _P4;
			float _P5;
			float _P6;





			fixed4 frag(v2f IN) : SV_Target
			{
				float4 color = float4(0,0,0,1);
				float2 u = _P1 * (IN.vertex.xy - _ScreenParams * 0.5) / _ScreenParams.x;
				float2x2 mat2 = { _CosTime.a, -_SinTime.a, _SinTime.a, _CosTime.a };
				u = mul(mat2,u);
				color.r = max(step(abs(fmod(u.x, 1)), _P2), step(abs(fmod(u.y, 1)), _P2));
				return color;
			}

			fixed4 frag_2(v2f IN) : SV_Target
			{
				fixed4 color = fixed4(0,0,0,1);
				fixed2 s = fixed2(1, 1);
				fixed2 u = 8 * (IN.vertex.xy) / _ScreenParams.x;
				fixed2 a = fmod(u, s) * 2 - s;
				fixed2 b = fmod(u + s * 0.5, s) * 2 - s;
				color.r = max(step(dot(a, a), 0.1), step(dot(b, b), 0.1));
				return color;
			}
		ENDCG
		}
	}
}
