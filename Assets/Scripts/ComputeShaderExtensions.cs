using System.Collections.Generic;
using UnityEngine;

namespace Graph
{
    public static class ComputeShaderExtensions
    {
        private static readonly Dictionary<string, int> properties = new Dictionary<string, int>();

        public static int GetId(this ComputeShader shader, string propertyName)
        {
            if (!properties.TryGetValue(propertyName, out int id))
            {
                id = Shader.PropertyToID(propertyName);
                properties.Add(propertyName, id);
            }

            return id;
        }

        public static ComputeKernel GetKernel(this ComputeShader shader, int kernelId)
        {
            return new ComputeKernel(shader, kernelId);
        }

        public static ComputeKernel GetKernel(this ComputeShader shader, string kernelName)
        {
            return new ComputeKernel(shader, kernelName);
        }
    }
}