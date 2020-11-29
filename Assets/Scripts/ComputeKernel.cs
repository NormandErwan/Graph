
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Graph
{
    public sealed class ComputeKernel
    {
        private int3? threadGroupSizes;

        public ComputeKernel(ComputeShader shader, int id)
        {
            Id = id;
            Shader = shader;
        }

        public ComputeKernel(ComputeShader shader, string name)
        {
            if (!shader.HasKernel(name))
            {
                throw new ArgumentException("The shader doesn't contain a kernel with this name.", nameof(name));
            }

            Id = shader.FindKernel(name);
            Shader = shader;
        }

        public int Id { get; }

        public ComputeShader Shader { get; }

        public int3 ThreadGroupSizes
        {
            get
            {
                if (threadGroupSizes == null)
                {
                    Shader.GetKernelThreadGroupSizes(Id, out uint x, out uint y, out uint z);
                    threadGroupSizes = (int3)new uint3(x, y, z);
                }
                return threadGroupSizes.Value;
            }
        }

        public void Dispatch(int3 threadGroups)
        {
            Dispatch(threadGroups.x, threadGroups.y, threadGroups.z);
        }

        public void Dispatch(int threadGroupsX, int threadGroupsY, int threadGroupsZ)
        {
            Shader.Dispatch(Id, threadGroupsX, threadGroupsY, threadGroupsZ);
        }

        public void SetTexture(int id, Texture texture)
        {
            Shader.SetTexture(Id, id, texture);
        }

        public void SetTexture(string name, Texture texture)
        {
            SetTexture(Shader.GetId(name), texture);
        }
    }
}