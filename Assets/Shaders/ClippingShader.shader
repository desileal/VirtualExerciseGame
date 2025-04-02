Shader "Universal Render Pipeline/ClippingShaderWithMaterial" {
    Properties {
        // Visible section material properties
        [Header(Visible Section)]
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Smoothness ("Smoothness", Range(0,1)) = 0.5

        // Clipping properties
        [Header(Clipping Settings)]
        _VisibleWidth ("Visible Width", Float) = 1.0
        _VisiblePosition ("Visible Position", Float) = 0.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        Pass {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                float4 tangentOS : TANGENT;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float3 tangentWS : TEXCOORD3;
                float3 bitangentWS : TEXCOORD4;
            };

            // Visible section properties
            sampler2D _MainTex;
            sampler2D _NormalMap;
            float4 _MainTex_ST;
            float4 _Color;
            float _Metallic;
            float _Smoothness;

            // Clipping properties
            float _VisibleWidth;
            float _VisiblePosition;

            Varyings vert(Attributes IN) {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                
                // Calculate TBN matrix for normal mapping
                VertexNormalInputs normalInput = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);
                OUT.normalWS = normalInput.normalWS;
                OUT.tangentWS = normalInput.tangentWS;
                OUT.bitangentWS = normalInput.bitangentWS;
                
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                // Calculate clipping bounds
                float leftBound = _VisiblePosition - (_VisibleWidth * 0.5);
                float rightBound = _VisiblePosition + (_VisibleWidth * 0.5);
                
                // Discard fragments outside visible area
                if (IN.worldPos.x < leftBound || IN.worldPos.x > rightBound) {
                    discard;
                }

                // Sample visible section material
                half4 albedo = tex2D(_MainTex, IN.uv) * _Color;
                half3 normalTS = UnpackNormal(tex2D(_NormalMap, IN.uv));

                // Transform normal to world space
                float3x3 TBN = float3x3(
                    IN.tangentWS,
                    IN.bitangentWS,
                    IN.normalWS
                );
                half3 normalWS = mul(normalTS, TBN);

                // Initialize SurfaceData fully
                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = albedo.rgb;
                surfaceData.alpha = albedo.a;
                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = _Smoothness;
                surfaceData.normalTS = normalTS;
                surfaceData.occlusion = 1.0; // Default occlusion
                surfaceData.emission = 0; // No emission

                // Initialize InputData fully
                InputData inputData = (InputData)0;
                inputData.positionWS = IN.worldPos;
                inputData.normalWS = normalWS;
                inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(IN.worldPos);
                inputData.shadowCoord = TransformWorldToShadowCoord(IN.worldPos);
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.positionHCS);

                return UniversalFragmentPBR(inputData, surfaceData);
            }
            ENDHLSL
        }
    }
}