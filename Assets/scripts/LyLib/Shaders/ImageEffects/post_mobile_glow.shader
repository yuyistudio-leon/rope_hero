Shader "Ly/Mobile_Glow" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
    Pass {
        ZTest Always Cull Off ZWrite Off
                
        CGPROGRAM
        // 使用默认的vertex函数即可。不能省略，否则无法正常工作。
        #pragma vertex vert_img
        #pragma fragment frag
        #include "UnityCG.cginc"

        uniform sampler2D _MainTex;
        uniform sampler2D _CameraDepthTexture;
        half strength_factor;
		float4 _MainTex_TexelSize;
		
        fixed4 frag (v2f_img i) : COLOR
        {        
			#if UNITY_UV_STARTS_AT_TOP
			//if (_MainTex_TexelSize.y < 0)
				 //i.uv.y = 1.0 - i.uv.y;
			#endif

			//half width = 0.01;
            half4 I = tex2D(_MainTex, i.uv);
			/*
            half4 I1 = tex2D(_MainTex, i.uv + float2(width, 0));
            half4 I2 = tex2D(_MainTex, i.uv + float2(0, width));
            half4 I3 = tex2D(_MainTex, i.uv + float2(-width, 0));
            half4 I4 = tex2D(_MainTex, i.uv + float2(0,-width));
			float4 color = (I + I1+I2+I3+I4) / 5;
			return color;     
			*/
			return I;
        }
        ENDCG

    }
}

Fallback off


}