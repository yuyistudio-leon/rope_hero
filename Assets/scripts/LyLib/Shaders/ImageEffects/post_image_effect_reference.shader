#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale' with 'float4(1,1,1,1)'
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
        // 使用默认的vertex函数即可。不能省略，否则无法正常工作。
        #pragma vertex vert_img
        // 使用sp变量（ScreenPos）
        #pragma target 3.0
        // 指定需要的fragment函数
        #pragma fragment frag
        // 标准头文件，里面有一些常用代码
        // 里面marco全部以UNITY_开头！
        #include "UnityCG.cginc"
        
        
        // 下面是内置的世界变量
        // The following built-in uniforms (except _LightColor0) 
		// are also defined in "UnityCG.cginc", 
		// i.e. one could #include "UnityCG.cginc" 
		uniform float4 _Time, _SinTime, _CosTime; // time values
		uniform float4 _ProjectionParams;
		// x = 1 or -1 (-1 if projection is flipped)
		// y = near plane; z = far plane; w = 1/far plane
		uniform float4 _ScreenParams; 
		// x = width; y = height; z = 1 + 1/width; w = 1 + 1/height
		uniform float4 float4(1,1,1,1); // w = 1/scale; see _World2Object
		// uniform float3 _WorldSpaceCameraPos;
		// uniform float4x4 _Object2World; // model matrix
		// uniform float4x4 _World2Object; // inverse model matrix 
		// (all but the bottom-right element have to be scaled 
		// with unity_Scale.w if scaling is important) 
		uniform float4 _LightPositionRange; // xyz = pos, w = 1/range
		uniform float4 _WorldSpaceLightPos0; 
		// position or direction of light source
		uniform float4 _LightColor0; // color of light source 
			
			
		// 内置常量
		内置矩阵
		支持的矩阵（float4x4）：

		UNITY_MATRIX_MVP        当前模型视图投影矩阵
		UNITY_MATRIX_MV           当前模型视图矩阵
		UNITY_MATRIX_V              当前视图矩阵。
		UNITY_MATRIX_P              目前的投影矩阵
		UNITY_MATRIX_VP            当前视图*投影矩阵
		UNITY_MATRIX_T_MV       移调模型视图矩阵
		UNITY_MATRIX_IT_MV      模型视图矩阵的逆转
		UNITY_MATRIX_TEXTURE0   UNITY_MATRIX_TEXTURE3          纹理变换矩阵
		内置载体
		Vectors (float4) supported:
		 向量（仅float4）支持：

		UNITY_LIGHTMODEL_AMBIENT        当前环境的颜色。




        uniform sampler2D _MainTex;// 声明_MainTex，不加uniform也可以
        uniform sampler2D _CameraDepthTexture;// 内置变量，就是深度缓存
        half strength_factor;
        
		// v2f_img是cginc里面的结构体

        fixed4 frag (v2f_img i) : COLOR
        {
        
            // 获取像素点的深度值（距离摄像机的距离）
            float d = UNITY_SAMPLE_DEPTH ( tex2D (_CameraDepthTexture, i.uv) ); // i.uv.xy??
            // 标准化距离，视椎体最远端为1，最近端为0.
            d = Linear01Depth (d);
            // 获取当前正在处理的像素
            half4 I = tex2D(_MainTex, i.uv);
            // 创建颜色
            half4 C = half4(1, 0, 0, 0);
            // 常用函数 distance pow lerp
            half dist = distance(float2(0.5,0.5),i.uv);
            return _LightColor0;// lerp(I, C, pow(dist, 1 / strength_factor));
        }
        ENDCG

    }
}

Fallback off


}