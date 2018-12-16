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
        half4 c = tex2D (_MainTex, IN.uv_MainTex);
		if(c.r <= 0.3){
			c.r = 0.2;
		}else if(c.r >0.3 && c.r < 0.7){
			c.r = 0.5;
		}else{
			c.r = 0.8;
		}
		if(c.g <= 0.3){
			c.g = 0.2;
		}else if(c.g >0.3 && c.g < 0.7){
			c.g = 0.5;
		}else{
			c.g = 0.8;
		}
		if(c.b <= 0.3){
			c.b = 0.2;
		}else if(c.b >0.3 && c.b < 0.7){
			c.b = 0.5;
		}else{
			c.b = 0.8;
		}
        
        o.Albedo = c.rgb;
        o.Normal = UnpackNormal ((tex2D (_BumpMap, IN.uv_BumpMap)) + (tex2D (_BumpMap2, IN.uv_BumpMap2)) *2-1);
      }
      ENDCG
    }
    Fallback "Diffuse"
  }