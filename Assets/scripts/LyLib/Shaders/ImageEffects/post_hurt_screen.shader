// 后期图像shader，对屏幕上的图像进行最终处理！不需要考虑光照效果！

Shader "Custom/PostHurtScreen" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}// 会被默认赋值为屏幕上的图像
        strength_factor ("Strength of red", Range(0.1,1)) = 1// 需要在脚本中进行赋值
    }
    SubShader {
    Pass {
        ZTest Always Cull Off ZWrite Off
                
        CGPROGRAM
        // 使用默认的vertex函数即可。不能省略，否则无法正常工作。
        #pragma vertex vert_img
        // 指定需要的fragment函数
        #pragma fragment frag
        // 标准头文件，里面有一些常用代码
        #include "UnityCG.cginc"

        uniform sampler2D _MainTex;// 声明_MainTex，不加uniform也可以
        half strength_factor;

		// v2f_img是cginc里面的结构体
        fixed4 frag (v2f_img i) : COLOR
        {
            half4 I = tex2D(_MainTex, i.uv);
            half4 C = half4(1, 0, 0, 0);
            half dist = distance(float2(0.5,0.5),i.uv);
            //return half4(dist,0,0,1);
            return lerp(I, C, pow(dist, 1 / strength_factor));
        }
        ENDCG

    }
}

Fallback off


}