Shader "Graph/Point"
{
    Properties
    {
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
    }

    SubShader
    {
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };

        float _Smoothness;

        void surf(Input input, inout SurfaceOutputStandard surface)
        {
            surface.Albedo.rg = input.worldPos.xy / 2 + 0.5;
            surface.Smoothness = _Smoothness;
        }

        ENDCG
    }

    FallBack "Diffuse"
}
