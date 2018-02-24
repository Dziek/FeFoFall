// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ScrollShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags
		{
			"PreviewType" = "Plane"
		}
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			float4 _MainTex_ST; 
			float4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				// o.uv = v.uv;
				
				o.uv = TRANSFORM_TEX( v.uv, _MainTex );
				//o.uv =  v.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				
				return o;
			}
			
			float4 frag(v2f i) : SV_Target
			{
				// float4 color = float4(1, 1 ,1 ,1);
				float4 color = tex2D(_MainTex, i.uv) * _Color;
				return color;
			}
			ENDCG
		}
	}
}