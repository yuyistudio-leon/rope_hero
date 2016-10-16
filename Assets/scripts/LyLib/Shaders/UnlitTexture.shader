// used to show leaves of plants
Shader "LyLib/UnlitTexture" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { 
			"Queue"="Geometry"
			"RenderType" = "Opaque"
		}
		LOD 200

		// use light
		Lighting Off
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types.
		// alpha: tell 'surf' that we want to use alpha channel of the texture(in another way, we can create transparent effect without using the alpha channel).
		#pragma surface surf Light

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		
		float4 LightingLight(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			return float4(s.Albedo, s.Alpha);
		}

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		sampler2D _MainTex;
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c * _Color.rgb * 2;
			o.Alpha = _Color.a * c.a * 2;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
