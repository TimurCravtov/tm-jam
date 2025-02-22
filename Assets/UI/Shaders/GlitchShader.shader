Shader "Custom/GlitchEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlitchInterval ("Glitch Interval", Range(0.0, 2.0)) = 0.5
        _DispIntensity ("Displacement Intensity", Range(0.0, 1.0)) = 0.1
        _ColorIntensity ("Color Split Intensity", Range(0.0, 1.0)) = 0.1
        _NoiseScale ("Noise Scale", Range(0.0, 100.0)) = 50.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 100
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _GlitchInterval;
            float _DispIntensity;
            float _ColorIntensity;
            float _NoiseScale;

            // Random function
            float random(float2 st)
            {
                return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }

            // Noise function
            float noise(float2 st)
            {
                float2 i = floor(st);
                float2 f = frac(st);
                
                float a = random(i);
                float b = random(i + float2(1.0, 0.0));
                float c = random(i + float2(0.0, 1.0));
                float d = random(i + float2(1.0, 1.0));

                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(a, b, u.x) + (c - a)* u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                
                // Time-based glitch effect
                float time = _Time.y;
                float glitchTime = floor(time / _GlitchInterval) * _GlitchInterval;
                
                // Generate noise
                float2 noiseCoord = uv * _NoiseScale;
                float noiseValue = noise(noiseCoord + glitchTime);
                
                // Displacement
                float2 displacement = float2(noiseValue, noiseValue) * _DispIntensity;
                displacement *= step(0.8, random(float2(glitchTime, 0)));
                
                // Color splitting
                float2 redOffset = displacement * _ColorIntensity;
                float2 blueOffset = -displacement * _ColorIntensity;
                
                // Sample colors with offset
                fixed4 colorR = tex2D(_MainTex, uv + redOffset);
                fixed4 colorG = tex2D(_MainTex, uv);
                fixed4 colorB = tex2D(_MainTex, uv + blueOffset);
                
                // Combine colors
                fixed4 finalColor = fixed4(colorR.r, colorG.g, colorB.b, colorG.a);
                
                // Add scanlines
                float scanline = sin(uv.y * 100 + time * 10) * 0.02;
                finalColor += scanline;
                
                return finalColor;
            }
            ENDCG
        }
    }
}