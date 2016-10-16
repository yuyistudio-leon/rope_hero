// one-color object for mobile games
Shader "LyLib/LightWallBKTexShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { 
			"Queue"="Geometry" 
			"RenderType"="Opaque"
		}
		LOD 200

		// use light
		Lighting On
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types.
		// alpha: tell 'surf' that we want to use alpha channel of the texture(in another way, we can create transparent effect without using the alpha channel).
		#pragma surface surf RegardlessLightDirection

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		
		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		sampler2D _MainTex;

		float4 LightingRegardlessLightDirection(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			float difLight = 1; //max(0, dot (s.Normal, lightDir));
			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (difLight * atten * 2);
			col.a = s.Alpha;
			return col;
		}
		
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = _Color.rgb * c.rgb;
			o.Alpha = _Color.a * c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
