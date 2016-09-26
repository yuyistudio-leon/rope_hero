// 后期图像shader，对屏幕上的图像进行最终处理！不需要考虑光照效果！
// 多个后期效果可以同时使用！
Shader "Custom/Underwater" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}// 会被默认赋值为屏幕上的图像
        strength_factor ("Strength of red", Range(0.1,1)) = 1// 需要在脚本中进行赋值
    }
    SubShader {
    Pass {
        ZTest Always Cull Off ZWrite Off
                
        CGPROGRAM
        // 使用默认的vertex函数
        #pragma vertex vert_img
        // 使用sp变量（ScreenPos）
        #pragma target 3.0
        // 指定需要的fragment函数
        #pragma fragment frag
        // 标准头文件，里面有一些常用代码
        // 里面marco全部以UNITY_开头！
        #include "UnityCG.cginc"

        uniform sampler2D _MainTex;// 声明_MainTex，不加uniform也可以
		uniform float4 _MainTex_TexelSize;// xy是main texture的尺寸的倒数，即x=1/width,y=1/height。所以下面的代码可以对周围像素进行采样
		uniform sampler2D _CameraDepthTexture;

		
        fixed4 frag (v2f_img i) : COLOR
        {
            float d = UNITY_SAMPLE_DEPTH ( tex2D (_CameraDepthTexture, i.uv) ); 
            d = Linear01Depth (d);
            
            float2 wave_size = float2(10,10);// x位水平波的宽度，y为垂直波的高度
            float vertical_wave_width = 2.0f;// 垂直波的宽度
            float horizontal_wave_height = 2.0f;// 垂直波的宽度
            wave_size *= _MainTex_TexelSize.xy;
            float2 wave_speed = float2(1, 2);
            float PI = 3.14;
            float2 ov = float2(
	            cos((i.uv.x + _Time.x * wave_speed.x) * PI * 2 * horizontal_wave_height),
            	sin((i.uv.y + _Time.x * wave_speed.y) * PI * 2 * vertical_wave_width)
            	) * wave_size;
            // 获取当前正在处理的像素
            half4 I = tex2D(_MainTex, i.uv + ov);
            // 计算光强
            float4 luminance = Luminance(I.rgb) * (1+I.a*2);
            return I;
        }
        ENDCG

    }
}

Fallback off


}