Shader "Unlit/IntersectionCutout"
{
     Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
 
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
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
            };
 
            struct v2f
            {
                float4 position : SV_POSITION;
            };
 
            sampler2D _MainTex;
 
            v2f vert(appdata v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                return o;
            }
 
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.position.xy / i.position.w);
 
                // Установка альфа-канала в 0 для пересекающейся геометрии
                col.a = 0;
 
                return col;
            }
            ENDCG
        }
    }
}