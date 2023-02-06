using System;
using UniRx;
using UnityEngine;
using Features.Ar.Data;

namespace Features.Ar.Models
{
    public class ArTrackingModel : IArTrackingStateProvider
    {
        private readonly ReactiveProperty<bool> _isTracked = new(false);

        private readonly ReactiveProperty<PositionData> _position = new(default);


        private Vector3[] _contentBoundsPositions = Array.Empty<Vector3>();
        public bool IsActive { get; private set; }
        public string ImageName { get; private set; }

        public void UpdateIsTracked(bool value, PositionData positionData = default, string imageName = "")
        {
            ImageName = imageName;

            _position.Value = positionData;
            _isTracked.Value = value;
        }

        public void UpdateContentBoundPositions(Vector3[] positions)
        {
            _contentBoundsPositions = positions;
        }

        public IObservable<bool> GetIsTrackedAsObservable()
        {
            return _isTracked.AsObservable();
        }

        public bool GetIsTracked()
        {
            return _isTracked.Value;
        }

        public IObservable<PositionData> GetPositionAsObservable()
        {
            return _position.AsObservable();
        }

        public PositionData GetPosition()
        {
            return _position.Value;
        }

        public Vector3[] GetContentBoundsPositions()
        {
            var result = new Vector3[_contentBoundsPositions.Length];
            for (int i = 0; i < _contentBoundsPositions.Length; i++)
            {
                result[i] = _position.Value.Position + _position.Value.Rotation * _contentBoundsPositions[i];
            }

            return result;
        }

        public void Enable()
        {
            IsActive = true;
        }

        public void Disable()
        {
            IsActive = false;
        }
    }
}