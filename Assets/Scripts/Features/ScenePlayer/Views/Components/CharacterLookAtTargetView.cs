using System;
using UnityEngine;
using UniRx;

// ReSharper disable once CheckNamespace
namespace Features.ScenePlayer.View
{
    public class CharacterLookAtTargetView : MonoBehaviour
    {
        [SerializeField] private Transform _character;
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 2f;

        private IDisposable _disposable;


        private void OnEnable()
        {
            _disposable = Observable.EveryUpdate()
                .Subscribe(_  =>
                {
                    _character.rotation = Quaternion.Euler(0, _character.rotation.eulerAngles.y, 0);

                    var rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _character.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

                    _character.rotation = Quaternion.Lerp(_character.rotation, rotation, _speed * Time.deltaTime);
                });
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
        }
    }
}