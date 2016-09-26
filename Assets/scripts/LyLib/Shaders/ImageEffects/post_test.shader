Shader "Custom/PostTest" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _factor ("factor", Range(0,100)) = 1
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
        float _factor;
		
        fixed4 frag (v2f_img i) : COLOR
        {
        	half2 uv = i.uv;
        	uv.x = (sin(uv.x * 10) + 1)/2;
        	uv.y = (cos(uv.y * 10) + 1)/2;
            return tex2D(_MainTex, uv);
        }
        ENDCG

    }
}

Fallback off


}