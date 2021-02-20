using System.Runtime.InteropServices;
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

        private static readonly int uvMapId = Shader.PropertyToID("UvMap");

        private RenderTexture debugTexture;
        private int3 graphKernelGroups;
        private ComputeBuffer positionsBuffer;
        private ComputeKernel positionsKernel;
        private ComputeBuffer uvMapBuffer;

        private void OnDisable()
        {
            debugTexture.Release();
            positionsBuffer.Release();
            uvMapBuffer.Release();
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

            var uvMap = Map.Linear((0, resolution), positionsInterval);
            uvMapBuffer = new ComputeBuffer(count: 1, stride: Marshal.SizeOf<LinearMapGpu>());
            uvMapBuffer.SetData(new LinearMapGpu[] { uvMap });
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
            graphShader.SetBuffer(positionsKernel.Id, uvMapId, uvMapBuffer);
            positionsKernel.Dispatch(graphKernelGroups);
        }
    }
}
