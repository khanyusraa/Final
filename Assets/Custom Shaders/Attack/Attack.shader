Shader "Custom/PulseBorderShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _PulseColor ("Pulse Color", Color) = (1,0,0,1)  // Red pulse color
        _PulseSpeed ("Pulse Speed", Float) = 1.0        // Speed of the pulsing effect
        _BorderThickness ("Border Thickness", Float) = 0.05 // Thickness of the border
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _PulseColor;
            float _PulseSpeed;
            float _BorderThickness;

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Fragment shader for pulsing the border
            fixed4 frag(v2f i) : SV_Target
            {
                // Get the base color from the texture
                fixed4 baseColor = tex2D(_MainTex, i.uv);

                // Calculate the pulse factor (0 to 1) using a sine wave for pulsing animation
                float pulseFactor = abs(sin(_Time.y * _PulseSpeed));

                // Calculate the distance of the current fragment from the border
                float distFromBorder = min(min(i.uv.x, 1.0 - i.uv.x), min(i.uv.y, 1.0 - i.uv.y));

                // Apply the pulsing effect only to the border (within the border thickness)
                float borderMask = smoothstep(_BorderThickness, _BorderThickness - 0.01, distFromBorder);

                // Blend the base color with the pulse color only at the border
                fixed4 pulseColor = lerp(baseColor, _PulseColor, borderMask * pulseFactor);

                return pulseColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}