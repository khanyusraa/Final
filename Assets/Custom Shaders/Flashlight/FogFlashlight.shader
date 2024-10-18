Shader "Custom/VolumetricFogFlashlight"
{
    Properties
    {
        _FogColor ("Fog Color", Color) = (0.5, 0.5, 0.5, 1) // Base fog color
        _Density ("Fog Density", Float) = 0.1 // Density of the fog
        _LightFalloff ("Light Falloff", Float) = 1.0 // Light falloff through the fog
        _MaxDistance ("Max Distance", Float) = 50.0 // Maximum distance of the fog effect
        _FogHeight ("Fog Height", Float) = 1.0 // Height at which fog becomes less dense
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        // Enable transparency blending
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off // Disable depth writing for transparency

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Properties
            float4 _FogColor;
            float _Density;
            float _LightFalloff;
            float _MaxDistance;
            float _FogHeight;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            // Vertex Shader
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // Convert to clip space
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // Get world position of the vertex
                return o;
            }

            // Raymarching function to calculate fog density
            float RaymarchFog(float3 startPos, float3 rayDir)
            {
                float fogAmount = 0.0;
                float stepSize = 0.1; // Step size for raymarching
                float totalDistance = 0.0;
                float maxDistance = _MaxDistance;

                // March along the ray
                for (int i = 0; i < 100; i++) // Number of iterations for raymarching
                {
                    if (totalDistance >= maxDistance) break; // Stop marching when we hit the max distance

                    float3 samplePos = startPos + rayDir * totalDistance;

                    // Calculate fog density based on the height and distance
                    float heightFactor = max(0.0, _FogHeight - samplePos.y);
                    float fogDensity = heightFactor * _Density;

                    fogAmount += fogDensity * stepSize; // Accumulate fog density

                    totalDistance += stepSize;
                }

                return saturate(fogAmount); // Ensure the fog amount stays between 0 and 1
            }

            // Fragment Shader
            fixed4 frag(v2f i) : SV_Target
            {
                float3 worldPos = i.worldPos;
                float3 cameraPos = _WorldSpaceCameraPos; // Get the camera position in world space

                // Calculate the direction of the ray from the camera to the fragment
                float3 rayDir = normalize(worldPos - cameraPos);

                // Perform raymarching to calculate the fog density
                float fogFactor = RaymarchFog(cameraPos, rayDir);

                // Apply light falloff to simulate the flashlight's effect on the fog
                float distance = length(worldPos - cameraPos);
                float falloff = exp(-_LightFalloff * distance); // Exponential falloff

                // Calculate the final fog color by multiplying the fog factor and falloff
                float4 fogColor = _FogColor * fogFactor * falloff;

                // Use the fog factor to control the transparency
                fogColor.a = fogFactor; // Set the alpha based on the fog density

                return fogColor; // Return the final color with transparency
            }

            ENDCG
        }
    }

    Fallback "Transparent"
}
