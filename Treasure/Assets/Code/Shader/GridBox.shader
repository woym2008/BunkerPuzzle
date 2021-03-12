Shader "Sprites/GridBox"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_P1("P1", Range(0,5)) = 1
		_P2("P2", Range(0,10)) = 1
		_P3("P3", Range(0,10)) = 4
		_P4("P4", Range(0,10)) = 2
		_P5("P5", Range(0,5)) = 0.5



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
		Blend One OneMinusSrcAlpha

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




			fixed4 frag(v2f IN) : SV_Target
			{
				/*
				float2  px = 4 * (2 * IN.vertex.xy - _ScreenParams.xy) / _ScreenParams.y;
				float id = 0.5 + 0.5 * cos(_Time.g+ sin(dot(floor(px + 0.5), float2(113.1, 17.81)))*43758.545);
				float3  co = 0.5 + 0.5*cos(_Time.g + 3.5*id + float3(0.0, 1.57, 3.14));
				float2  pa = smoothstep(0.0, 0.2, id*(0.5 + 0.5*cos(6.2831*px)));
				float4 c = float4(co*pa.x*pa.y, 1.0);*/
				//
				float2 u = _P3 * (IN.vertex.xy) / _ScreenParams.xy;
				float2 s = float2(1, _P1);
				float2 a = fmod(u, s) * _P4 - s * _P4 / 2;
				float2 b = fmod(u + s * _P5, s) * _P4 - s * _P4 / 2;
				float c = min(dot(a, a),dot(b,b));
				return step(c , _P2);

				return float(_P1 * c);
				float color = float(_P1 * min( dot(a, a), dot(b, b) ));
				return color;
			}
		ENDCG
		}
	}
}
