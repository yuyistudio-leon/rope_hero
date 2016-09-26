// 在垂直方向上逐渐变得透明
Shader "LyLib/Fade_VerticalFade" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_PosFade0 ("Fade0", float) = 0.0
		_PosFade1 ("Fade1", float) = 1.0
	}
	SubShader {
		Tags { 
			"IgnoreProjector"="True" 
			"RenderType"="Opaque"
			"Queue"="Transparent" 
		}
		LOD 200
		Lighting On
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float _PosFade0, _PosFade1;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			float alpha = pow(saturate((IN.worldPos.y - _PosFade0) / abs(_PosFade1 - _PosFade0)), 1);
			float fade = saturate(alpha) * _Color.a;
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb * fade;
			o.Alpha = fade;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
