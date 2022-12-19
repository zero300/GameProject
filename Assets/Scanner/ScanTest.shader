Shader "Custom/ScanTest"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always


        Pass 
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appData {
                float4 vertex :POSITION;
                float2 uv : TEXCOORD0;
                float4 ray : TEXCOORD1;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 ray : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float3 _ScanCenterPos;
            float _ScanWidth;
            float4 _HeadColor;
            float4 _TrailColor;

            v2f vert (appData v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.ray = v.ray;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex , i.uv);
                float depth = Linear01Depth(DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv)));
                float4 toCameraVector = depth * i.ray;
                float3 worldPos = _WorldSpaceCameraPos + toCameraVector;

                float outer = _ScanWidth;
                float distanceToCenter = distance(_ScanCenterPos, worldPos);
                float value = smoothstep(0, outer, distanceToCenter);
                fixed4 ringColor;
                if (value >= 1 || value <= 0) {
                    value = 0;
                    ringColor = float4(1, 1, 1, 1);
                    return col;
                }
                else
                {
                    ringColor = lerp(_HeadColor , _TrailColor, value);
                    
                }
                return ringColor;
            }
            ENDCG
        }
    }
}
