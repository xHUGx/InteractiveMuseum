using UnityEngine;

namespace Features.App.Views
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private Camera camera;

        public Transform Transform => transform;
        public Camera Camera => camera;
    }
}