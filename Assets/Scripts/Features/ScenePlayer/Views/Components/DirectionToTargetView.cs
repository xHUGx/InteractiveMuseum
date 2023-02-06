using System;
using UnityEngine;
using UniRx;

// ReSharper disable once CheckNamespace
namespace Features.ScenePlayer.View
{
    public class DirectionToTargetView : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 2f;

        private IDisposable _disposable;

        private void OnEnable()
        {
            _disposable = Observable.EveryUpdate()
                .Subscribe(_  =>
                {
                    var direction = transform.position - _target.position;
                    var rotation = Quaternion.LookRotation(direction);
                    _target.rotation = Quaternion.Lerp(_target.rotation, rotation, _speed * Time.deltaTime);
                });
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
        }
    }
}