using System;
using Features.Ar.Models;
using UniRx;
using UnityEngine;

namespace Features.Ar.Services
{
    public class PointsInCameraFrustumService : IDisposable
    {
        private const int UpdateInterval = 50;
        private const int OutOfFrustumTimeMax = 2500;

        private readonly ArComponentsModel _arComponentsModel;

        private Subject<Unit> _isOutOfFrustumSubject;

        private IDisposable _followStream;

        public IObservable<Unit> GetIsOutOfFrustumAsObservable() => _isOutOfFrustumSubject;

        private float _outOfFrustumTime;

        public PointsInCameraFrustumService(ArComponentsModel arComponentsModel)
        {
            _arComponentsModel = arComponentsModel;
            _isOutOfFrustumSubject = new Subject<Unit>();
        }

        public bool TryStartFollow(Vector3[] positions)
        {
            if (!_arComponentsModel.IsReady) return false;

            _outOfFrustumTime = 0f;

            _followStream?.Dispose();

            _followStream =
                Observable
                    .Interval(TimeSpan.FromMilliseconds(UpdateInterval))
                    .Subscribe(_ =>
                    {
                        var checkResult = true;
                        foreach (var position in positions)
                        {
                            checkResult = checkResult && CheckIsPointOutFrustum(_arComponentsModel.CameraView.Camera, position);
                        }

                        // Debug.Log($"[PointsInCameraFrustumService] Is content in camera frustum: {!checkResult}" );
                        if (checkResult)
                        {
                            _outOfFrustumTime += UpdateInterval;
                            if (_outOfFrustumTime < OutOfFrustumTimeMax) return;
                            StopFollow();
                            _isOutOfFrustumSubject.OnNext(new Unit());
                        }

                        _outOfFrustumTime = 0;
                    });
            return true;
        }

        public void StopFollow()
        {
            _followStream?.Dispose();
        }

        public void Dispose()
        {
            StopFollow();
        }

        private bool CheckIsPointOutFrustum(Camera camera, Vector3 worldPoint)
        {
            var viewPoint = camera.WorldToViewportPoint(worldPoint);
            var isInDirection = CheckIsInDirection(camera.transform, worldPoint);
            var cameraRectContains = camera.rect.Contains(viewPoint);
            return !(isInDirection && cameraRectContains);
        }

        private bool CheckIsInDirection(Transform from, Vector3 to)
        {
            var fromCameraToTarget = (to - from.position).normalized;
            var cameraDirection = from.forward.normalized;
            var dot = Vector3.Dot(fromCameraToTarget, cameraDirection);

            return dot > 0.5f;
        }
    }
}