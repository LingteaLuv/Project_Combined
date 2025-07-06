Shader "Custom/FogOfWar"
{
    Properties
    {
        _MainTex("Map Texture", 2D) = "white" {}
        _FogTexture("Fog Texture", 2D) = "white" {}
        _FogColor("Fog Color", Color) = (0, 0, 0, 0.8)
        _FogIntensity("Fog Intensity", Range(0, 1)) = 0.8
    }

        SubShader
        {
            Tags {
                "RenderType" = "Transparent"
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
            }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                sampler2D _FogTexture;
                fixed4 _FogColor;
                float _FogIntensity;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // 기본 맵 텍스처 (있다면)
                    fixed4 mapColor = tex2D(_MainTex, i.uv);

                // 안개 텍스처 (R8 포맷이므로 r 채널 사용)
                fixed fogMask = tex2D(_FogTexture, i.uv).r;

                // 안개 색상 설정
                fixed4 fogColor = _FogColor;
                fogColor.a *= _FogIntensity;

                // 안개가 있는 부분(흰색=1)은 안개색, 없는 부분(검은색=0)은 투명
                // lerp(투명, 안개색, 안개마스크)
                fixed4 result = lerp(fixed4(0, 0, 0, 0), fogColor, fogMask);

                return result;
            }
            ENDCG
        }
        }

            FallBack "Sprites/Default"
}
