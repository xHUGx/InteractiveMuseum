using UnityEngine;

namespace Features.LightSystem.Views
{
    public class DirectionalLightHandlerView : MonoBehaviour
    {
        public Transform Transform => transform;

        public Vector3 GetForwardDirection()
        {
            return transform.forward;
        }
    }
}