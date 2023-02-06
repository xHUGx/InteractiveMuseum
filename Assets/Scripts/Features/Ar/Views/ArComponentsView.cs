using Features.App.Views;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR.ARFoundation;

namespace Features.ImageTracking.Views
{
    public class ArComponentsView : MonoBehaviour
    {
        [SerializeField] private ARSession _arSession;
        [SerializeField] private ARSessionOrigin _arSessionOrigin;
        [SerializeField] private ARTrackedImageManager _arTrackedImageManager;
        [SerializeField] private ARCameraManager _arCameraManager;
        [SerializeField] private CameraView _cameraView;
        [SerializeField] private TrackedPoseDriver _trackedPoseDriver;

        public ARSession ArSession => _arSession;
        public ARSessionOrigin ArSessionOrigin => _arSessionOrigin;
        public ARTrackedImageManager ArTrackedImageManager => _arTrackedImageManager;
        public ARCameraManager ArCameraManager => _arCameraManager;
        public CameraView CameraView => _cameraView;
        public Transform Transform => transform;
        public TrackedPoseDriver TrackedPoseDriver => _trackedPoseDriver;

        public void Dispose()
        {
            try
            {
                Destroy(gameObject);
            }
            catch
            {
                // ignored
            }
        }
    }
}