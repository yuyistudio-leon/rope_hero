// one-color object for mobile games
Shader "LyLib/LightWallShader" {
	Properties {
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
			float3 worldPos;
		};

		fixed4 _Color;

		float4 LightingRegardlessLightDirection(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			half light_strength = abs( dot (s.Normal, float3(0, 0, 1)) );
			if (light_strength > 0.1){
				light_strength = 0.1;
			}else {
				light_strength = 1;
			}

			float difLight = 1; //max(0, dot (s.Normal, lightDir));
			float4 col;
			col.rgb = s.Albedo * _LightColor0.rgb * (difLight * atten * 2) * light_strength;
			col.a = s.Alpha;
			return col;
		}
		
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = float3(1,1,1);
			o.Alpha = 1;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
