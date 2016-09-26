Shader "Custom/PostMist" {
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
        uniform float _MaxDist;
        fixed4 frag (v2f_img i) : COLOR
        {
			half4 mist_color = half4(0.8,0.8,0.2,0);
            half4 I = tex2D(_MainTex, i.uv);
            half d = UNITY_SAMPLE_DEPTH ( tex2D (_CameraDepthTexture, half2(i.uv.x, 1-i.uv.y)) ); // i.uv.xy??
            d = Linear01Depth (d);
//            return I + d * mist_color;// 变亮
	        return I - d * mist_color;// 变暗
        }
        ENDCG

    }
}

Fallback off


}