Shader "UI/GridLight"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

		_GradientTex("Gradient Texture", 2D) = "white" {}
		_FlowLight("FlowLight", Color) = (0.3,0.3,0,0.3)
		_Width("Width", Range(0,1)) = 0.1

		_TickWidth("TickWidth", Range(0,1)) = 0.2
		_GridWidth("GridWidth", Range(0,1)) = 0.01

		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]


		Pass
		{
			Name "Default"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
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
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
			};
			
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;
			float _Width;
			float _TickWidth;
			float _GridWidth;

			float mod(float  a, float  b)
			{
				return a - b * floor(a / b);
			}

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0) * float2(-1,1) * OUT.vertex.w;
				#endif
				
				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _GradientTex;
			fixed4 _FlowLight;

			fixed4 frag(v2f IN) : SV_Target
			{
				//half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

				float2 r = 2.0*(IN.texcoord - 0.5);
				float4 backgroundColor = float4(0, 0, 0, 0);;
				float4 gridColor = float4(0, 0.5, 0.9, 1);	
				float4 color = backgroundColor;
				float a = 0;
				//定义网格的的间距  
				const float tickWidth = _TickWidth;
				if (mod(r.x, tickWidth) < _GridWidth)
				{
					color = gridColor;
				}

				if (mod(r.y, tickWidth) < _GridWidth)
				{
					color = gridColor;
				}

				if (abs(color.x) == backgroundColor.x
					&& abs(color.y) == backgroundColor.y
					&& abs(color.z) == backgroundColor.z
					&& abs(color.w) == backgroundColor.w
					)
				{
					discard;
				}
		
				if (color.a != 0.0) {
					float v = tex2D(_GradientTex, IN.texcoord).r;
					float diff = v - _CosTime.g - 0.5;
					float save_alpha = color.a;
					color.a = 0;
					if (abs(diff) < _Width) {
						color.a = save_alpha;
						color.a = color.a - lerp(0.5, 1, abs(diff) / _Width);
					}
				}

				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
}
