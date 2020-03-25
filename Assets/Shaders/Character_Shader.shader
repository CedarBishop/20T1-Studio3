Shader "Custom/Dissolve"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Intensity("Intensity", Float) = 10
		_EdgeColor("Edge Color", Color) = (1,0,0.7064714,0)
		_FresnelPower("Fresnel Power", Float) = 5
		_FresnelColor("Fresnel Color", Color) = (1,1,1,0)

		/* Dissolve effect Properties */
		_DissolveTex("Dissolve Texture", 2D) = "white" {}
		_DissolveBorderColour1("Dissolve Border 1", Color) = (1,0,0,1)
		_DissolveBorderColour2("Dissolve Border 2", Color) = (1,1,1,0.25)
		_DissolveAmount ("Dissolve Level", Range (0, 1)) = 0
		_DissolveWidth ("Dissolve Width", Range (0.0, 1.0)) = 0.025
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
        	Lighting Off
        	ZWrite Off
        	Fog { Mode Off }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile

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
			float4 _MainTex_ST;
			float _Intensity;
			fixed4 _EdgeColor;

			/* Dissolve properties initialization */
			sampler2D _DissolveTex;
			float4 _DissolveBorderColour1;
			float4 _DissolveBorderColour2;
			float _DissolveAmount;
			float _DissolveWidth;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float cutout = tex2D(_DissolveTex, i.uv).r;
				fixed4 col = tex2D(_MainTex, i.uv);

				if (cutout < _DissolveAmount)
					discard;

				if(cutout < col.a && cutout < _DissolveAmount + _DissolveWidth)
					col =lerp(_DissolveBorderColour1, _DissolveBorderColour2, (cutout - _DissolveAmount) / _DissolveWidth );

				return col;
			}
			ENDCG
		}
	}

    Fallback "Diffuse"
}
