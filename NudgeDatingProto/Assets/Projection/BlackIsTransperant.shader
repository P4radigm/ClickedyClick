Shader "Custom/BlackIsTransperant"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("Threshold", Range(0, 1)) = 0.1
        _Tiling ("Tiling", Vector) = (1, 1, 1, 1)
        _Offset ("Offset", Vector) = (0, 0, 0, 0)
    }
 
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 100
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
 
            struct appdata_t
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
            float _Threshold;
            float4 _Tiling;
            float4 _Offset;
 
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _Tiling.xy + _Offset.xy;
                return o;
            }
 
            half4 frag (v2f i) : SV_Target
            {
                half4 texColor = tex2D(_MainTex, i.uv);
                
                // Check if the pixel is "totally black" based on the threshold value
                if (dot(texColor.rgb, float3(1, 1, 1)) < _Threshold)
                {
                    // Make it transparent
                    discard;
                }
 
                return texColor;
            }
            ENDCG
        }
    }
}