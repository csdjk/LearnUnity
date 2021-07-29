//create by 长生但酒狂
//漫反射 - 在片元着色器计算
Shader "lcl/learnShader1/002_Diffuse_fargment" {
    //属性
    Properties{
        _Diffuse("Diffuse Color",Color) = (1,1,1,1)
        _Roughness ("Roughness", Range(0, 10)) = 1.0
        
    }
    SubShader {
        Pass{
            Tags { "LightMode"="ForwardBase" }
            CGPROGRAM
            #include "Lighting.cginc"
            #pragma vertex vert
            #pragma fragment frag


            struct a2v {
                float4 vertex : POSITION;
                float3 normal: NORMAL;
            };

            struct v2f{
                float4 position:SV_POSITION;
                float3 worldNormalDir:COLOR0;
            };

            v2f vert(a2v v){
                v2f f;
                f.position = UnityObjectToClipPos(v.vertex);
                f.worldNormalDir = mul(v.normal,(float3x3) unity_WorldToObject);
                return f;
            };

            float4 _Diffuse;
            float _Roughness;

            fixed4 frag(v2f f):SV_TARGET{
                // 环境光
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
                //法线
                fixed3 normalDir = normalize(f.worldNormalDir);
                //灯光
                fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                //漫反射计算
                fixed3 diffuse = _LightColor0.rgb * max(dot(normalDir,lightDir),0);
                fixed3 resultColor = (diffuse+ambient) * _Diffuse;


                fixed3 reflectDir = reflect(-lightDir,normalDir);//反射光

                
                float cosTheta = dot(normalDir,lightDir);

                float oneMinusCosL = 1.0f - abs(cosTheta);
                float oneMinusCosLSqr = oneMinusCosL * oneMinusCosL;
                float oneMinusCosV = 1.0f - abs(cosTheta);
                float oneMinusCosVSqr = oneMinusCosV * oneMinusCosV;

                // Roughness是粗糙度，IDotH的意思会在下一篇讲Microfacet模型时提到
                float IDotH = dot(lightDir, normalize(lightDir + reflectDir));
                float F_D90 = 0.5f + 2.0f * IDotH * IDotH * _Roughness;


                float INV_PI = 1/3.14159;


                return INV_PI * (1.0f + (F_D90 - 1.0f) * oneMinusCosLSqr * oneMinusCosLSqr * oneMinusCosL) *
                (1.0f + (F_D90 - 1.0f) * oneMinusCosVSqr * oneMinusCosVSqr * oneMinusCosV);
            };
            
            ENDCG
        }
    }
    FallBack "VertexLit"
}
