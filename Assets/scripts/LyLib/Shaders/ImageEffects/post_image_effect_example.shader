// Upgrade NOTE: commented out 'float3 _WorldSpaceCameraPos', a built-in variable
// Upgrade NOTE: commented out 'float4x4 _Object2World', a built-in variable
// Upgrade NOTE: commented out 'float4x4 _World2Object', a built-in variable

// 后期图像shader，对屏幕上的图像进行最终处理！不需要考虑光照效果！
// 多个后期效果可以同时使用！
Shader "Custom/ImageEffectTestShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}// 会被默认赋值为屏幕上的图像
        strength_factor ("Strength of red", Range(0.1,1)) = 1// 需要在脚本中进行赋值
    }
    SubShader {
    Pass {
        ZTest Always Cull Off ZWrite Off
                
        CGPROGRAM
        // 使用默认的vertex函数
        //#pragma vertex vert_img
        // 使用自定义vertex函数
		#pragma vertex vert
        // 使用sp变量（ScreenPos）
        #pragma target 3.0
        // 指定需要的fragment函数
        #pragma fragment frag
        // 标准头文件，里面有一些常用代码
        // 里面marco全部以UNITY_开头！
        #include "UnityCG.cginc"
        
        // 自定义vertex2fragment结构体
		struct v2f {
			float4 pos : SV_POSITION;// 自定义位置
			float2 uv : TEXCOORD0;
			float2 uv_custom: TEXCOORD0; // 自定义的uv
		};
        
        uniform sampler2D _MainTex;// 声明_MainTex，不加uniform也可以
		uniform float4 _MainTex_TexelSize;// xy是main texture的尺寸的倒数，即x=1/width,y=1/height。所以下面的代码可以对周围像素进行采样
		//float top_pixel = tex2D(_MainTex, uv + float2(ox,oy) * _MainTex_TexelSize.xy;
        uniform sampler2D _CameraDepthTexture;// 内置变量，就是深度缓存
        half strength_factor;
        uniform float4 sp: WPOS;
		// v2f_img是cginc里面的结构体

		v2f vert( appdata_img v ) {
			v2f o;
			o.pos = mul (UNITY_MATRIX_MVP, v.vertex);// 计算世界坐标
			
			float2 uv = v.texcoord.xy;
			o.uv.xy = uv;
			o.uv_custom.xy = ComputeScreenPos(o.pos) * _MainTex_TexelSize.xy;// 计算屏幕坐标（左下角为0,0，右上角为1,1）.
			return o;
		}
        fixed4 frag (v2f i) : COLOR
        {
        	//float4 screen_pos = ComputeScreenPos(i.pos);
        	//float2 screen_pos_percent = screen_pos.xy;
            // 获取像素点的深度值（距离摄像机的距离）
            float d = UNITY_SAMPLE_DEPTH ( tex2D (_CameraDepthTexture, i.uv) ); // i.uv.xy??
            // 标准化距离，视椎体最远端为1，最近端为0.
            d = Linear01Depth (d);
            // 获取当前正在处理的像素
            half4 I = tex2D(_MainTex, i.uv);
            // 计算光强
            float4 luminance = Luminance(I.rgb) * (1+I.a*2);
			// 把uv用颜色显示出来
            float4 uv_color =  float4(i.uv_custom, 0, 1);
            // 常用函数 distance pow lerp
            half dist = distance(float2(0.5,0.5),i.uv);
            return luminance * I;
        }
        ENDCG

    }
}

Fallback off


}