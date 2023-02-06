using EffectSystem;
using EffectSystem.Data;
using EffectSystem.View;
using UnityEngine;
using Zenject;
using DG.Tweening;

// ReSharper disable once CheckNamespace
namespace Features.ScenePlayer.View
{
    public class FruitView : MonoBehaviour
    {
        [SerializeField] private Transform _startTransform;
        [SerializeField] private Transform _targetTransform;

        [SerializeField] private GameObject _fruit;
        [SerializeField] private GameObject _trail;

        [SerializeField] private bool _yFloor = true;
        [SerializeField] private Ease _yEase = Ease.InSine;
        [SerializeField] private bool _withEffect = true;
        [SerializeField] private float _duration = 1f;

        private const float _trailFinishDuration = 0.30f;

        private EffectsController _effectsController;
        private Sequence _sequence;

        private const float FinishYPosition = 0.04f;

        [Inject]
        private void Construct(EffectsController effectsController)
        {
            _effectsController = effectsController;
        }

        private void OnEnable()
        {
            transform.position = _startTransform.position;
            transform.rotation = _startTransform.rotation;

            _fruit.SetActive(true);
            _trail.SetActive(true);

            _sequence = DOTween.Sequence()
                .Append(transform.DOMoveX(_targetTransform.position.x, _duration).SetEase(Ease.Linear))
                .Join(transform.DOMoveZ(_targetTransform.position.z, _duration).SetEase(Ease.Linear));

            if (_yFloor)
                _sequence.Join(transform.DOLocalMoveY(FinishYPosition, _duration).SetEase(_yEase));
            else
                _sequence.Join(transform.DOMoveY(_targetTransform.position.y, _duration).SetEase(_yEase));

            _sequence
                .AppendCallback(StartEffect)
                .AppendCallback(() => _fruit.SetActive(false))
                .AppendInterval(_trailFinishDuration)
                .AppendCallback(() => _trail.SetActive(false));
        }

        private void OnDisable()
        {
            _sequence?.Kill();
        }

        private void StartEffect()
        {
            if (_effectsController != null && _withEffect)
                _effectsController.ShowEffectWithInput<FruitExplosionEffect, FruitExplosionEffect.Data>
                    (new FruitExplosionEffect.Data() { Position = transform.position, Rotation = _targetTransform.rotation }, EffectLayer.UnderUI);
        }
    }
}