Shader "Custom/PostTV" {
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
		
        fixed4 frag (v2f_img i) : COLOR
        {
			half width = 0.01;
            half4 I = tex2D(_MainTex, i.uv);
            half4 C = half4(0.1, 0.2, 0.15, 0) * (_SinTime.w + 1) / 2;
            float factor = abs(fmod(i.uv.y, width) / width - 0.5) * 2;
            //return half4(fmod(i.uv.x,0.1) * 10, 0, 0, 1);
          return lerp(I, C, factor);          
        }
        ENDCG

    }
}

Fallback off


}