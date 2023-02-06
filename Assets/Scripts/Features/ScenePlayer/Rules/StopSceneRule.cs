using System;
using Features.App.Controllers;
using Features.Ar.Messages;
using Features.ScenePlayer.Models;
using Rule;
using UniRx;
using Features.App.Data;
using Features.Ar.Models;
using Features.ScenePlayer.Messages;
using Zenject;

namespace Features.ScenePlayer.Rules
{
    public class StopSceneRule : IInitializable, IDisposable //AbstractSignalRule<ArSignals.ImageLost>
    {
        private readonly ScenePlayerModel _scenePlayerModel;
        private readonly IAppModel _appModel;
        private readonly IArTrackingStateProvider _arTrackingState;
        private readonly SignalBus _signalBus;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private IDisposable _arTrackingStream;
        public StopSceneRule(SignalBus signalBus, ScenePlayerModel scenePlayerModel, IAppModel appModel,
            IArTrackingStateProvider arTrackingState)
        {
            _signalBus = signalBus;
            _scenePlayerModel = scenePlayerModel;
            _appModel = appModel;
            _arTrackingState = arTrackingState;
            
        }

        // protected override void OnSignalFired(ArSignals.ImageLost signal)
        // {
        //     StopScene();
        // }

        public  void Initialize()
        {
            // base.Initialize();

            _signalBus
                .GetStream<ScenePlayerSignals.SceneFinished>()
                .Subscribe(_ => { StopScene(); })
                .AddTo(_disposables);
            
            _appModel
                .GetAppStateAsObservable()
                .Where(appState => appState.AppState == AppStateType.ScenePlayer)
                .Subscribe(appState =>
                {
                    if (appState.EventType == StateEventType.Enter)
                    {
                        _arTrackingStream = _arTrackingState
                            .GetIsTrackedAsObservable()
                            .Where(isTracked => !isTracked)
                            .Subscribe(_ => StopScene());

                    }
                    if (appState.EventType == StateEventType.Exited)
                    {
                        StopScene();
                        _arTrackingStream?.Dispose();
                    }
                    
                    // else if (value.EventType == StateEventType.Stay && !_arTrackingModel.GetIsTracked())
                    // {
                    //     StopScene();
                    // }
                })
                .AddTo(_disposables);
        }

        private void StopScene()
        {
            // _arTrackingModel.UpdateIsTracked(false);
            _scenePlayerModel.UpdateIsPlaying(false);
        }

        public void Dispose()
        {
            // base.Dispose();

            _disposables?.Dispose();
        }
    }
}