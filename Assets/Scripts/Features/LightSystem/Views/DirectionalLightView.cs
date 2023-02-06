using System;
using UnityEngine;

namespace Features.LightSystem.Views
{
    public class DirectionalLightView : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Light _light;
        public Transform Transform => transform;

        public Light Light => _light;


        public void UpdateForwardDirection(Vector3 forward)
        {
            transform.forward = forward;
        }

        public void Dispose()
        {
            try
            {
                Destroy(gameObject);
            }
            catch
            {
                // Ignored
            }
        }
    }
}