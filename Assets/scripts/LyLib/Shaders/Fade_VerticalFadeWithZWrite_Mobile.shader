// 在垂直方向上逐渐变得透明
Shader "LyLib/Fade_Mobile_VerticalFadeWithZWrite" {
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
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha

        Pass {

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
		
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "AutoLight.cginc"

		float _PosFade0, _PosFade1;
        fixed4 _Color;
        sampler2D _MainTex;
        float4 _MainTex_ST;

        struct v2f {
            float4 svPos : SV_POSITION;
			float4 worldPos : TANGENT;
            float2 uv : TEXCOORD0;

			float3 worldNormal : NORMAL;
			float3 lightDir : TEXCOORD2;
			LIGHTING_COORDS(3,4)
        };


        v2f vert (appdata_base v)
        {
            v2f o;
            o.worldPos = mul (_Object2World, v.vertex);    // 转换到世界坐标系
            o.svPos = mul(UNITY_MATRIX_MVP, v.vertex);    // 转换到世界坐标系
            o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);    // 处理该uv的offset和scale设置
			o.worldNormal = mul((float3x3)_Object2World, SCALED_NORMAL);
//			o.worldNormal = mul(_Object2World, v.normal).xyz;

			o.lightDir = mul((float3x3)_Object2World, ObjSpaceLightDir(v.vertex));
					
			// pass lighting information to pixel shader
  			TRANSFER_VERTEX_TO_FRAGMENT(o);
            return o;
        }

        fixed4 frag (v2f i) : SV_Target
        {
			// get texture
			fixed3 tex_color = tex2D(_MainTex, i.uv);

			// calculate light strength
			fixed3 ambi = UNITY_LIGHTMODEL_AMBIENT.xyz;
			fixed atten = LIGHT_ATTENUATION(i);
			fixed3 diff = _LightColor0.rgb * saturate (dot (normalize(i.worldNormal),  normalize(i.lightDir))) * 2;

			// calculate alpha according to the worldPos.y
			float alpha = pow(saturate((i.worldPos.y - _PosFade0) / abs(_PosFade1 - _PosFade0)), 1);
			
			// calculate final color
			fixed4 c;
			c.rgb = float3((ambi + diff * atten) * tex_color) * _Color.rgb;
			c.a = alpha * _Color.a;
			c.r = c.r * 0.7 + 0.3 * ((1 + _CosTime.w) * 0.3 + 0.2);
			return c;
        }
        ENDCG

        }
    }
}