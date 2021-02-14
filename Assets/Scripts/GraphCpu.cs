using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Graph
{
    public sealed class GraphCpu : MonoBehaviour
    {
        [SerializeField]
        private Transform pointPrefab = default;

        [SerializeField]
        private Interval positionsInterval = (-1, 1);

        [SerializeField]
        [Range(10, 100)]
        private int resolution = 10;

        [SerializeField]
        private GraphFunction function = GraphFunction.Plane;

        private readonly Dictionary<GraphFunction, Functions.Function> functions
            = new Dictionary<GraphFunction, Functions.Function>()
        {
            { GraphFunction.MultiWave, Functions.MultiWave },
            { GraphFunction.Plane, Functions.Plane },
            { GraphFunction.Ripple, Functions.Ripple },
            { GraphFunction.Sphere, Functions.Sphere },
            { GraphFunction.Torus, Functions.Torus },
            { GraphFunction.TwistedSphere, Functions.TwistedSphere },
            { GraphFunction.TwistedTorus, Functions.TwistedTorus },
            { GraphFunction.Wave, Functions.Wave },
        };

        private readonly Dictionary<float2, Transform> points = new Dictionary<float2, Transform>();

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                Clear();
                Init();
            }
        }

        private void Update()
        {
            var position = functions[function];

            foreach (var (uv, point) in points)
            {
                point.transform.localPosition = (float3)position(uv, Time.time);
            }
        }

        private void Init()
        {
            var uvMap = Map.Linear((0, resolution), positionsInterval);
            var scale = new float3(2) / resolution;

            for (int i = 0; i <= resolution; i++)
            {
                for (int j = 0; j <= resolution; j++)
                {
                    var point = Instantiate(pointPrefab);
                    var uv = new float2(uvMap[i], uvMap[j]);

                    points.Add(uv, point);

                    point.name = $"({uv.x:N2}, {uv.y:N2})";

                    point.SetParent(transform, worldPositionStays: false);
                    point.transform.localScale = scale;
                }
            }
        }

        private void Clear()
        {
            foreach (var (_, point) in points)
            {
                Destroy(point.gameObject);
            }
            points.Clear();
        }
    }
}
