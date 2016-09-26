Shader "Custom/MistShader" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
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

        fixed4 frag (v2f_img i) : COLOR
        {
            half blurFactor;
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
            //return C;
            
            float d = UNITY_SAMPLE_DEPTH ( tex2D (_CameraDepthTexture, i.uv) ); // i.uv.xy??
            d = Linear01Depth (d);
            blurFactor=saturate(abs(d-focalDistance01));
            //    blurFactor=saturate((IN.depth-_FocalDistance)*(IN.depth-_FocalDistance)*1.5);
            //    if(blurFactor<0.2)
            //        blurFactor*=blurFactor*3;
            //return fixed4(blurFactor);
            return lerp(I,C,blurFactor);
        }
        ENDCG

    }
}

Fallback off


}