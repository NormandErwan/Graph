using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace Graph
{
    public static class Functions
    {
        public delegate float3 Function(float2 uv, float t);

        public static float3 MultiWave(float2 uv, float t)
        {
            float z = sin(PI * (uv.x + 0.5f * t));
            z += 0.5f * sin(2f * PI * (uv.y + t));
            z += sin(PI * (uv.x + uv.y + 0.25f * t));
            z *= (1f / 2.5f);
            return float3(uv, z);
        }

        public static float3 Plane(float2 uv, float t)
        {
            return float3(uv, z: 0);
        }

        public static float3 Ripple(float2 uv, float t)
        {
            float d = sqrt(uv.x * uv.x + uv.y * uv.y);
            float z = sin(PI * (4f * d - t)) / (1f + 10f * d);
            return float3(uv, z);
        }

        public static float3 Sphere(float2 uv, float t)
        {
            float r = cos(PI * uv.y / 2);
            return Sphere(uv, 1, r, t);
        }

        public static float3 Torus(float2 uv, float t)
        {
            return Torus(uv, r1: 1, r2: 0.5f, t);
        }

        public static float3 TwistedSphere(float2 uv, float t)
        {
            float r = 0.9f + 0.1f * sin(PI * (6f * uv.x + 4f * uv.y + t));
            float s = cos(PI * uv.y / 2);
            return Sphere(uv, r, s, t);
        }

        public static float3 TwistedTorus(float2 uv, float t)
        {
            float r1 = 0.7f + 0.1f * sin(PI * (6f * uv.x + 0.5f * t));
            float r2 = 0.15f + 0.05f * sin(PI * (8f * uv.x + 4f * uv.y + 2f * t));
            return Torus(uv, r1, r2, t);
        }

        public static float3 Wave(float2 uv, float t)
        {
            float z = sin(PI * (uv.x + uv.y + t));
            return float3(uv, z);
        }

        private static float3 Sphere(float2 uv, float r, float s, float t)
        {
            return float3(
                x: s * sin(PI * uv.x),
                y: s * cos(PI * uv.x),
                z: r * sin(PI * uv.y / 2)
            );
        }

        private static float3 Torus(float2 uv, float r1, float r2, float t)
        {
            float s = r1 + r2 * cos(PI * uv.y);
            return float3(
                x: s * sin(PI * uv.x),
                y: s * cos(PI * uv.x),
                z: r2 * sin(PI * uv.y)
            );
        }
    }
}