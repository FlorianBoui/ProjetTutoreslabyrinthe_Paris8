  Shader "Example/Diffuse Bump" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _BumpMap ("Bumpmap", 2D) = "bump" {}
	  _BumpMap2 ("Bumpmap2", 2D) = "bump2" {}
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert
      struct Input {
        float2 uv_MainTex;
        float2 uv_BumpMap;
		float2 uv_BumpMap2;
      };
      sampler2D _MainTex;
	  sampler2D _BumpMap;
      sampler2D _BumpMap2;
      void surf (Input IN, inout SurfaceOutput o) {
        o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * (0.55,0.55,0.55);
        o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
		o.Normal = UnpackNormal (tex2D (_BumpMap2, IN.uv_BumpMap2));
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }