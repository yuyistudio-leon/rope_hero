// one-color object for mobile games
Shader "LyLib/WallShader" {
	Properties {
		_StrengthTex ("Strength (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_StartZ ("Start Z", float) = 4
		_ScaleX ("Scale X", float) = 1
		_ScaleY ("Scale Y", float) = 1
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
		#pragma surface surf NoLight
		
		fixed4 _Color;
		float _StartZ;
		float4 LightingNoLight(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			half light_strength = abs( dot (s.Normal, float3(0, 0, 1)) );
			if (light_strength > 0.1){
				light_strength = 0.03;
				}else {
				light_strength = 1;
				}
			float3 color = s.Albedo * atten * light_strength;
			return float4(color, 1);
		}

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		
		struct Input {
			float3 worldPos;
		};

		float _ScaleX;
		float _ScaleY;
		sampler2D _StrengthTex;
		void surf (Input IN, inout SurfaceOutput o) {
			float2 uv = float2(IN.worldPos.x * _ScaleX, IN.worldPos.z * _ScaleY);
			fixed4 c = tex2D (_StrengthTex, uv) * _Color;
			float strength = 0.7 + (c.r + c.g + c.b) / 3;

			float z = 1 + (IN.worldPos.z - _StartZ) / _StartZ; // 将 (0, -StartZ) 映射到 (0,1)
			float factor = (z + 0.3) * 0.6 * strength;
			o.Albedo = float3(_Color.r * factor, _Color.g * factor, _Color.b * factor);
			//o.Albedo = c;
			o.Alpha = _Color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
