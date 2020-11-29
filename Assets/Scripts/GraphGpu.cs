using Unity.Mathematics;
using UnityEngine;

namespace Graph
{
    public sealed class GraphGpu : MonoBehaviour
    {
        [SerializeField]
        private ComputeShader graphShader;

        [SerializeField]
        private Interval positionsInterval = (-1, 1);

        [SerializeField]
        [Range(10, 1000)]
        private int resolution = 10;

        [SerializeField]
        private GraphFunction function = GraphFunction.Plane;

        [SerializeField]
        private Material debugMaterial = default;

        private static readonly int
            intervalMaxId = Shader.PropertyToID("IntervalMax"),
            intervalMinId = Shader.PropertyToID("IntervalMin"),
            textureId = Shader.PropertyToID("Texture");

        private int graphKernelId;
        private int3 graphKernelGroups;
        private ComputeBuffer positionsBuffer;
        private RenderTexture debugTexture;

        private void OnEnable()
        {
            int sizeofPosition = 3 * sizeof(float);
            positionsBuffer = new ComputeBuffer(resolution * resolution, sizeofPosition);

            graphKernelId = graphShader.FindKernel("Main");
            graphShader.GetKernelThreadGroupSizes(graphKernelId, out uint x, out uint y, out uint z);
            graphKernelGroups = (int3)math.ceil(new float3(resolution, resolution, 1) / new float3(x, y, z));

            debugTexture = new RenderTexture(resolution, resolution, 0)
            {
                enableRandomWrite = true
            };
            debugTexture.Create();
            graphShader.SetTexture(graphKernelId, textureId, debugTexture);
            debugMaterial.mainTexture = debugTexture;
        }

        private void OnDisable()
        {
            positionsBuffer.Release();
            debugTexture.Release();
        }

        private void Update()
        {
            graphShader.SetFloat(intervalMinId, positionsInterval.Min);
            graphShader.SetFloat(intervalMaxId, positionsInterval.Max);

            graphShader.Dispatch(graphKernelId, graphKernelGroups.x, graphKernelGroups.y, graphKernelGroups.z);
        }
    }
}