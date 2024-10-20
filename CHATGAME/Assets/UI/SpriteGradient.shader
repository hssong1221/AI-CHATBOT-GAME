Shader "UI/Gradient"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Power("Alpha Power", Range(0, 5)) = 2.0
        _Direction("Gradient Direction (0: Bottom to Top, 1: Top to Bottom)", Range(0, 1)) = 1.0
    }
        SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            ZTest Always
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

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

            fixed4 _Color;
            float _Power;
            float _Direction;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 그라데이션 방향에 따라 uv.y 값을 반대로 적용
                float uvY = lerp(i.uv.y, 1.0 - i.uv.y, _Direction);
                float alpha = pow(uvY, _Power); // 중간에서 더 빨리 불투명해지도록 설정
                return fixed4(_Color.rgb, _Color.a * alpha);
            }
            ENDCG
        }
    }
}

