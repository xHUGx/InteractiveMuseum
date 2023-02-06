using System;
using Features.Ar.Models;
using UnityEngine;
using Zenject;
using UniRx;

namespace Features.UI.View
{
    public class DebugContentBoundsPositionsView : MonoBehaviour
    {
        private ArTrackingModel _arTrackingModel;

        private Vector3[] _points;

        [Inject]
        private void Construct(ArTrackingModel arTrackingModel)
        {
            _arTrackingModel = arTrackingModel;

            _arTrackingModel
                .GetIsTrackedAsObservable()
                .Subscribe(value =>
                {
                    _points = null;
                    if (value)
                        _points = _arTrackingModel.GetContentBoundsPositions();
                })
                .AddTo(this);
        }


        private void OnDrawGizmos()
        {
            if (_points == null) return;
            foreach (var point in _points)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(point, 0.02f);
            }
            
        }
    }
}