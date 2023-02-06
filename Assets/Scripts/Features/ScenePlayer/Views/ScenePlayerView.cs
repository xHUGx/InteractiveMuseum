#if UNITY_EDITOR
using Features.ScenePlayer.Attributes;
#endif
using Features.Ar.Data;
using Features.Ar.Models;
using System;
using Features.Ar.Views;
using Features.LightSystem.Messages;
using Features.LightSystem.Views;
using UnityEngine;
using Zenject;
using UniRx;
using UnityEngine.Playables;
using Features.ScenePlayer.Messages;

namespace Features.ScenePlayer.Views
{
    public class ScenePlayerView : MonoBehaviour, IScenePlayer
    {
#if UNITY_EDITOR
        [AttributeScenePlayerName]
#endif
        [SerializeField]
        private string _sceneName;

        [SerializeField] private PlayableDirector _playableDirector;

        [SerializeField] private ArImageAnchorView _arImageAnchorView;
        [SerializeField] private DirectionalLightHandlerView directionalLightHandlerView;

        private IArTrackingStateProvider _arTrackingState;
        private SignalBus _signalBus;

        private IDisposable _disposable;

        [Inject]
        private void Construct(IArTrackingStateProvider arTrackingState, SignalBus signalBus)
        {
            _arTrackingState = arTrackingState;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            StopScene();
            
            if (directionalLightHandlerView != null)
            {
                _signalBus.TryFire(
                    new LightSignals.RegisterNewDirectionalLightHandler(_sceneName, directionalLightHandlerView));
            }
        }

        public string SceneName => _sceneName;


        public void PlayScene()
        {
            gameObject.SetActive(true);

            UpdatePositionByRelativeAnchor(_arTrackingState.GetPosition());

            _disposable = _arTrackingState.GetPositionAsObservable()
                .Subscribe(data => { UpdatePositionByRelativeAnchor(data); });

            _playableDirector.stopped += OnPlayableDirectorStopped;

            _playableDirector.Play();
        }

        public void StopScene()
        {
            if (_playableDirector != null)
            {
                _playableDirector.stopped -= OnPlayableDirectorStopped;
                _playableDirector.Stop();
            }

            gameObject.SetActive(false);
            _disposable?.Dispose();
        }

        private void OnPlayableDirectorStopped(PlayableDirector playableDirector)
        {
            _signalBus.Fire(new ScenePlayerSignals.SceneFinished());
        }

        public void UpdatePositionByRelativeAnchor(PositionData positionData = default)
        {
            var deltaPosition = Quaternion.Inverse(transform.rotation) *
                                (transform.position - _arImageAnchorView.Transform.position);

            transform.rotation = positionData.Rotation *
                                 Quaternion.Inverse(Quaternion.Inverse(transform.rotation) *
                                                    _arImageAnchorView.Transform.rotation);

            transform.position = positionData.Position + transform.rotation * deltaPosition;
        }

        private void ChangePosition(PositionData positionData)
        {
            // transform.position = positionData.Position;
            // transform.rotation = positionData.Rotation;
        }
    }
}