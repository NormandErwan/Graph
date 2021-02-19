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

        private static readonly int intervalMaxId = Shader.PropertyToID("IntervalMax");
        private static readonly int intervalMinId = Shader.PropertyToID("IntervalMin");
        private static readonly int textureId = Shader.PropertyToID("Texture");

        private int3 graphKernelGroups;
        private ComputeBuffer positionsBuffer;
        private ComputeKernel positionsKernel;
        private RenderTexture debugTexture;

        private void OnDisable()
        {
            positionsBuffer.Release();
            debugTexture.Release();
        }

        private void OnEnable()
        {
            int sizeofPosition = 3 * sizeof(float);
            positionsBuffer = new ComputeBuffer(resolution * resolution, sizeofPosition);

            positionsKernel = graphShader.GetKernel("Main");

            int3 positionsWorkSize = new int3(resolution, resolution, 1);
            graphKernelGroups = positionsKernel.GetThreadGroups(positionsWorkSize);

            debugTexture = new RenderTexture(resolution, resolution, 0)
            {
                enableRandomWrite = true
            };
            debugTexture.Create();
            positionsKernel.SetTexture("Texture", debugTexture);
            debugMaterial.mainTexture = debugTexture;
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                OnDisable();
                OnEnable();
            }
        }

        private void Update()
        {
            graphShader.SetFloat(intervalMinId, positionsInterval.Min);
            graphShader.SetFloat(intervalMaxId, positionsInterval.Max);

            positionsKernel.Dispatch(graphKernelGroups);
        }
    }
}
