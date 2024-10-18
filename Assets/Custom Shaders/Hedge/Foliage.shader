Shader "Custom/Foliage"
{
    Properties
    {
        _TexScale ("Tex Scale", Range (0.1, 10.0))= 1.0
        _BlendPlateau ("BlendPlateau", Range (0.0, 1.0)) = 0.2       
        _MainTex ("Base Texture (RGB) Gloss(A)", 2D) = "white" {}
        _BumpMap1 ("Normal Map (_Y_X)", 2D)  = "bump" {}   
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

        // Wind Properties
        _WindSpeed ("Wind Speed", Float) = 1.0
        _WindFrequency ("Wind Frequency", Float) = 0.5
        _WindAmplitude ("Wind Amplitude", Float) = 0.1
        _WindXScale ("X Wind Effect", Range(0.0, 0.5)) = 0.05
        _WindZScale ("Z Wind Effect", Range(0.0, 0.5)) = 0.05

        // Color Property
        _Color ("Tint Color", Color) = (1, 1, 1, 1)

        // Additional properties for maze ambience shader
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _AlphaMask ("Alpha Mask", 2D) = "white" {}
        _ShadowIntensity ("Shadow Intensity", Range(0, 1)) = 0.5
        _FlickerSpeed ("Flicker Speed", Range(0.1, 5.0)) = 1.0
        _FlickerIntensity ("Flicker Intensity", Range(0, 1)) = 0.5
        _MoveSpeed ("Shadow Movement Speed", Range(0.1, 2.0)) = 0.5
    }
   
    SubShader
    {
        Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque"}
        LOD 400

        CGPROGRAM
        #pragma target 3.0                               
        #pragma surface surf Lambert vertex:vertLocal alphatest:_Cutoff

        sampler2D _MainTex, _BumpMap1;          
        half _TexScale, _BlendPlateau;

        // Wind Properties
        float _WindSpeed, _WindFrequency, _WindAmplitude;
        float _WindXScale, _WindZScale;

        // Color Property
        fixed4 _Color;

        // Maze Ambience Properties
        sampler2D _NoiseTex;
        sampler2D _AlphaMask;
        float _ShadowIntensity;
        float _FlickerSpeed;
        float _FlickerIntensity;
        float _MoveSpeed;

        struct Input
        {
            float3 thisPos;        
            float3 thisNormal;
            half4 color : COLOR; 
            float2 uv_NoiseTex;
            float2 uv_AlphaMask;
            INTERNAL_DATA
        };                    

        // Vertex function for wind effect
        void vertLocal (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.thisNormal = v.normal;
            o.thisPos = v.vertex;
            o.color = v.color;

            // UVs for noise texture and alpha mask
            o.uv_NoiseTex = v.texcoord;
            o.uv_AlphaMask = v.texcoord;

            // Wind Effect
            float time = _Time.y * _WindSpeed;

            // Wind displacement on X, Y, and Z axes
            float windX = sin(v.vertex.z * _WindFrequency + time) * _WindXScale;
            float windY = sin(v.vertex.x * _WindFrequency + time) * _WindAmplitude;
            float windZ = sin(v.vertex.y * _WindFrequency + time) * _WindZScale;

            // Displace the vertex positions
            v.vertex.x += windX;
            v.vertex.y += windY;
            v.vertex.z += windZ;
        }        

        void surf (Input IN, inout SurfaceOutput o)
        {                    
            // Determine the blend weights for the 3 planar projections    
            half3 blend_weights = abs(IN.thisNormal.xyz);
            blend_weights = (blend_weights - _BlendPlateau);
            blend_weights = max(blend_weights, 0);
            blend_weights /= (blend_weights.x + blend_weights.y + blend_weights.z).xxx;

            // Blended color and bump vectors
            half4 blended_color;
            half3 blended_bumpvec;

            // Compute UV coordinates for planar projections
            half2 coord1 = IN.thisPos.yz * _TexScale;  
            half2 coord2 = IN.thisPos.zx * _TexScale;  
            half2 coord3 = IN.thisPos.xy * _TexScale;  

            // Sample color maps for each projection
            half4 col1 = tex2D(_MainTex, coord1);
            half4 col2 = tex2D(_MainTex, coord2);
            half4 col3 = tex2D(_MainTex, coord3);

            // Sample bump maps and generate bump vectors
            half2 bumpVec1 = tex2D(_BumpMap1, coord1).wy * 2 - 1;  
            half2 bumpVec2 = tex2D(_BumpMap1, coord2).wy * 2 - 1;  
            half2 bumpVec3 = tex2D(_BumpMap1, coord3).wy * 2 - 1; 

            half3 bump1 = half3(0, bumpVec1.x, bumpVec1.y);  
            half3 bump2 = half3(bumpVec2.y, 0, bumpVec2.x);  
            half3 bump3 = half3(bumpVec3.x, bumpVec3.y, 0);

            // Blend the results of the 3 planar projections
            blended_color = col1 * blend_weights.xxxx +  
                            col2 * blend_weights.yyyy +  
                            col3 * blend_weights.zzzz;  

            blended_bumpvec = bump1 * blend_weights.xxx +  
                                bump2 * blend_weights.yyy +  
                                bump3 * blend_weights.zzz;  

            half4 c = blended_color;

            // Maze Ambience Effects
            // Noise texture for shadow and lighting movement
            float2 noiseUV = IN.uv_NoiseTex + float2(_MoveSpeed * _Time.y, _MoveSpeed * _Time.y);
            float noiseValue = tex2D(_NoiseTex, noiseUV).r;

            // Shadow movement effect (oscillating shadows)
            float shadowFactor = sin(_Time.y * _MoveSpeed + noiseValue) * _ShadowIntensity;
            c.rgb -= shadowFactor;

            // Flickering light effect using noise
            float flickerValue = sin(_Time.y * _FlickerSpeed + noiseValue) * _FlickerIntensity;
            c.rgb *= (1.0 + flickerValue); // Apply flickering light effect

            // Alpha masking for subtle hedge movement effects
            float alphaMaskValue = tex2D(_AlphaMask, IN.uv_AlphaMask + noiseValue).r;
            c.a *= alphaMaskValue;

            // Apply the color tint and texture color
            o.Albedo = c.rgb * _Color.rgb * IN.color.rgb;

            o.Alpha = c.a;  
            o.Normal = normalize(half3(0, 0, 1) + blended_bumpvec);        
        }
        ENDCG
    }

    FallBack "Diffuse"

}