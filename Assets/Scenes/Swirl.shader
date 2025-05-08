Shader "Custom/ChaoticSwirlURP"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (0.08,0.9,0.65,1)
        _BgColor("Background Color", Color) = (0,0,0,1)
        _Speed("Spin Speed", Float) = 1
        _Density("Stripe Density", Float) = 12
        _Swirl("Swirl Amount", Float) = 4
        _NoiseScale("Noise Scale", Float) = 8
        _NoiseStrength("Noise Strength", Float) = 2
        _NoiseTimeScale("Noise Time Scale", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode"="UniversalForward" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            // *** SIMPLE HASH / VALUE NOISE HELPERS ***
            float hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);
                return frac(p.x * p.y);
            }
            float valueNoise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float a = hash21(i);
                float b = hash21(i + float2(1, 0));
                float c = hash21(i + float2(0, 1));
                float d = hash21(i + float2(1, 1));
                float2 u = f * f * (3.0 - 2.0 * f); // smoothstep
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _BgColor;
                float _Speed;
                float _Density;
                float _Swirl;
                float _NoiseScale;
                float _NoiseStrength;
                float _NoiseTimeScale;
            CBUFFER_END
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }
            float4 frag(Varyings IN) : SV_Target
            {
                // remap uv so center is (0,0)
                float2 uv = IN.uv;
                float2 center = float2(0.5, 0.5);
                float2 diff = uv - center;
                float radius = length(diff) + 1e-5;
                // canonical angle (‑PI..PI)
                float angle = atan2(diff.y, diff.x);
                // spin over time
                angle += _Time.y * _Speed;
                // classic swirl proportional to radius
                angle += radius * _Swirl;
                // **** CHAOS: add animated value‑noise to the angle ****
                // sample low‑frequency noise in polar space for chunky blobs
                float n = valueNoise(uv * _NoiseScale + _Time.y * _NoiseTimeScale);
                // remap to ‑1..1 range then scale
                n = (n * 2.0 - 1.0) * _NoiseStrength;
                angle += n;
                // final stripes pattern
                float stripes = sin(angle * _Density) * 0.5 + 0.5; // 0..1
                float3 col = lerp(_BgColor.rgb, _BaseColor.rgb, stripes);
                return float4(col, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}















