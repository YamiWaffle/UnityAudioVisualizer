Shader "XRKTV/SpecturmMeter"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}

    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _SpecturmDataTex;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

            // get specturm data
            half specturmData = tex2D(_SpecturmDataTex, half2(i.uv.x, 0)).r;

            // Way 1
            //half level = round(i.uv.y / 0.125) * 0.125;
            //col.rgb *= step(level, specturmData);

            // Way 2
            //col.rgb = saturate(col.rgb* specturmData)* step(i.uv.y, specturmData);
            
            // Way 3
            col.rgb *= step(i.uv.y, specturmData); 
            col.a = 1;

            return col;
         }
         ENDCG
     }
    }
}
