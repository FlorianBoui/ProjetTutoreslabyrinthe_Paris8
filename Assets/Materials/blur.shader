Shader "Hidden/DepthOfField/MedianFilter"
{

	Properties
	{
		_MainTex ("-", 2D) = "black"

	}

			CGINCLUDE
			#pragma target 4.0
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			uniform float4 _Offsets;

			struct v2f
			{
				half4 pos  : SV_POSITION;
				half2 uv   : TEXCOORD0;
			};

			v2f vert(appdata_img v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				#if UNITY_UV_STARTS_AT_TOP
					if (_MainTex_TexelSize.y < 0)
						o.uv.y = 1-o.uv.y;
				#endif
				return o;
			}



			float4 fragCocMedian3x3 (v2f i) : SV_Target
			{
					half4 center = tex2Dlod(_MainTex, half4(i.uv + half2(0, 0) * _MainTex_TexelSize.xy,0,0));
					half4 result = (0.0,0.0,0.0,0.0);
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(-2, -2) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(-2, -1) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(-2, 0) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(-2, 1) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(-2, 2) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(-1, -2) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(-1, -1) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(-1, 0) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(-1, 1) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(-1, 2) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(0, -2) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(0, -1) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += (center.rgb * 0.8);
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(0, 1) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(0, 2) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(1, -2) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(1, -1) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(1, 0) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(1, 1) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(1, 2) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(2, -2) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(2, -1) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(2, 0) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(2, 1) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb += tex2Dlod(_MainTex, half4(i.uv + half2(2, 2) * _MainTex_TexelSize.xy,0,0)).rgb;
					result.rgb /= 25;
					return result;
			}
			ENDCG

	SubShader
	{
		Pass{
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment fragCocMedian3x3
        ENDCG
        
		}
	}
	Fallback "Diffuse"
}