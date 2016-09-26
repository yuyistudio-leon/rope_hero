Shader "Custom/ClipDof" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        blur_dist("Distance of totally blur", Range(0,1)) = 0.8
        blur_interpolation_dist ("Distance of blur interpolation", Range(0,1)) = 0.35
    }
    SubShader {
    Pass {
        ZTest Always Cull Off ZWrite Off
        //Fog { Mode off }
                
        CGPROGRAM
        #pragma vertex vert_img
        #pragma fragment frag
        //#pragma fragmentoption ARB_precision_hint_fastest 
        #include "UnityCG.cginc"

        uniform sampler2D _MainTex;
        uniform sampler2D _CameraDepthTexture;
        uniform half _OffsetDistance;
        uniform half focalDistance01;
        float blur_dist;// 完全模糊的距离
        float blur_interpolation_dist;// 从完全模糊到部分模糊的过度距离

        fixed4 frag (v2f_img i) : COLOR
        {

            float d = UNITY_SAMPLE_DEPTH ( tex2D (_CameraDepthTexture, i.uv) ); // i.uv.xy??
            d = Linear01Depth (d);
            half4 I = tex2D(_MainTex, i.uv);
            
            //fixed4 original = tex2D(_MainTex, i.uv);
            half4 C = float4(I);
            C =tex2D(_MainTex,half2(i.uv.x+_OffsetDistance,i.uv.y+_OffsetDistance));
            C+=tex2D(_MainTex,half2(i.uv.x+_OffsetDistance,i.uv.y-_OffsetDistance));
            C+=tex2D(_MainTex,half2(i.uv.x-_OffsetDistance,i.uv.y+_OffsetDistance));
            C+=tex2D(_MainTex,half2(i.uv.x-_OffsetDistance,i.uv.y-_OffsetDistance));
            
            C+=tex2D(_MainTex,half2(i.uv.x+_OffsetDistance,i.uv.y));
            C+=tex2D(_MainTex,half2(i.uv.x,i.uv.y+_OffsetDistance));
            C+=tex2D(_MainTex,half2(i.uv.x-_OffsetDistance,i.uv.y));
            C+=tex2D(_MainTex,half2(i.uv.x,i.uv.y-_OffsetDistance));
            
	        C*=0.05;
            
        	float min_blur_dist = blur_dist - blur_interpolation_dist;
            if (d > blur_dist){
	            return C;
	        }else if (d < blur_dist && d > min_blur_dist){
	        	return lerp(I, C, (d - min_blur_dist) / blur_interpolation_dist);
	        }else{
	        	return I;
	        }
        }
        ENDCG

    }
}

Fallback off


}