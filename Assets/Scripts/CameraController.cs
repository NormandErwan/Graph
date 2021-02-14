
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Graph
{
    public sealed class CameraController : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField]
        private Camera camera = null;

        [SerializeField]
        private Transform target = null;

        [Header("Zoom")]
        [SerializeField]
        private float zoomFactor = 1;

        [SerializeField]
        private Interval distanceInterval = (0, 1);

        private void OnValidate()
        {
            if (camera != null && target != null)
            {
                camera.transform.LookAt(target);
            }
        }

        public void Zoom(InputAction.CallbackContext context)
        {
            float direction = -math.sign(context.ReadValue<float>());
            if (direction != 0)
            {
                float3 targetToCamera = camera.transform.position - target.position;
                float distance = math.length(targetToCamera) + zoomFactor * direction;
                distance = distanceInterval.Clamp(distance);
                camera.transform.position = (float3)target.position + math.normalize(targetToCamera) * distance;
            }
        }
    }
}
