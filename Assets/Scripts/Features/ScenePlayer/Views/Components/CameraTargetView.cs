using System;
using Features.Ar.Models;
using UnityEngine;
using UniRx;
using Zenject;

// ReSharper disable once CheckNamespace
namespace Features.ScenePlayer.View
{
    public class CameraTargetView : MonoBehaviour
    {
        private Transform _cameraTransform;
        private IDisposable _disposable;

        private ArComponentsModel _arComponentsModel;

        [Inject]
        private void Construct(ArComponentsModel imageTrackingComponentsController)
        {
            _arComponentsModel = imageTrackingComponentsController;
        }

        private void OnEnable()
        {
            var cameraView = _arComponentsModel.CameraView;

            if (cameraView == null)
                return;

            _cameraTransform = cameraView.Transform;

            _disposable = Observable.EveryUpdate()
                .Subscribe(_  =>
                {
                    transform.position = _cameraTransform.position;
                    transform.rotation = _cameraTransform.rotation;
                });
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
        }
    }
}