Shader "Unlit/MagnifyMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius("Radius", float) = 1.0
        _Distortion("Distortion", float) = 5.0
        _Debug("Debug", int) = 0
        _P1("P1", float) = 2.0
        _P2("P2", float) = 4.0
        _M1("M1", float) = 5.0
        _M2("M2", float) = 2.0
        _PositionCount("PositionCount", int) = 5
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Radius;
            float _Distortion;
            int _Debug;
            float _P1;
            float _P2;
            float _M1;
            float _M2;

            int _PositionCount;
            float4 _Positions[5];
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float smax(float a, float b) {
                return  max(a,b);
            }

            float2 getCircle(float2 uv, float2 position)
            {
                float2 tileOffset = uv - position;
                float distance = saturate(1.0 - length(1/_Radius * tileOffset));
                float p1 = pow(distance, _P1);
                float p2 = pow(distance, _P2) * _M1;
                float r = (p1 - p2) * _M2;
                return (tileOffset * r * _Distortion);
            }

            fixed4 frag (v2f input) : SV_Target
            {
                float2 uv = input.uv;
                float2 sum;
                for(int i = 0; i < _PositionCount; i++)
                {
                    // float4 position = _Positions[i];
                    // if(length(position) ==0 ) continue;
                    float2 c = getCircle(uv, _Positions[i]);
                    sum += c;
                }
                uv  -=  sum;
                // // sample the texture
                fixed4 col = tex2D(_MainTex, uv);
                UNITY_APPLY_FOG(i.fogCoord, col);
                if(_Debug > 0)
                {
                    col = float4(uv.x,uv.y, 0.0, 1.0);
                }
                return col;
            }
            ENDCG
        }
    }
}
