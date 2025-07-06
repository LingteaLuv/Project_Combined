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
                    // �⺻ �� �ؽ�ó (�ִٸ�)
                    fixed4 mapColor = tex2D(_MainTex, i.uv);

                // �Ȱ� �ؽ�ó (R8 �����̹Ƿ� r ä�� ���)
                fixed fogMask = tex2D(_FogTexture, i.uv).r;

                // �Ȱ� ���� ����
                fixed4 fogColor = _FogColor;
                fogColor.a *= _FogIntensity;

                // �Ȱ��� �ִ� �κ�(���=1)�� �Ȱ���, ���� �κ�(������=0)�� ����
                // lerp(����, �Ȱ���, �Ȱ�����ũ)
                fixed4 result = lerp(fixed4(0, 0, 0, 0), fogColor, fogMask);

                return result;
            }
            ENDCG
        }
        }

            FallBack "Sprites/Default"
}
