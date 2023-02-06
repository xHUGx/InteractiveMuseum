using System;
using Features.ImageTracking.Views;
using Features.App.Views;
using UniRx;
using UnityEngine.XR.ARFoundation;

namespace Features.Ar.Models
{
    public class ArComponentsModel
    {
        private ArComponentsView _arComponentsView;
        private readonly ReactiveProperty<bool> _isReady = new ();
        public bool IsReady => _arComponentsView != null;
        public ARSession ArSession => IsReady ? _arComponentsView.ArSession : null;
        public ARSessionOrigin ARSessionOrigin => IsReady ? _arComponentsView.ArSessionOrigin : null;

        public IObservable<bool> GetIsReadyAsObservable() => _isReady;
        public ARTrackedImageManager ArTrackedImageManager =>
            IsReady ? _arComponentsView.ArTrackedImageManager : null;

        public ARCameraManager ArCameraManager => IsReady ? _arComponentsView.ArCameraManager : null;
        public CameraView CameraView => IsReady ? _arComponentsView.CameraView : null;

        public void Bind(ArComponentsView arComponentsView)
        {
            _arComponentsView = arComponentsView;
            _isReady.Value = true;
        }

        public void UnBind()
        {
            _isReady.Value = false;
            _arComponentsView = null;
        }
    }
}