using System;
using Features.App.Controllers;
using Features.App.Data;
using UniRx;
using Zenject;
using Features.Ar.Models;
using Features.ScenePlayer.Models;

namespace Features.ScenePlayer.Rules
{
    public class PlaySceneRule : IInitializable, IDisposable
    {
        private readonly IAppModel _appModel;
        private readonly IArTrackingStateProvider _arTrackingState;
        private readonly ScenePlayerModel _scenePlayerModel;

        private IDisposable _disposable;

        public PlaySceneRule(IAppModel appModel,
            ScenePlayerModel scenePlayerModel,
            IArTrackingStateProvider arTrackingState)
        {
            _appModel = appModel;
            _arTrackingState = arTrackingState;
            _scenePlayerModel = scenePlayerModel;
        }

        public void Initialize()
        {
            _disposable = _appModel
                .GetAppStateAsObservable()
                .Where(value => value.AppState == AppStateType.ScenePlayer &&
                                value.EventType == StateEventType.Stay)
                .Subscribe(value =>
                {
                    if (_arTrackingState.GetIsTracked())
                    {
                        _scenePlayerModel.UpdateIsPlaying(true, _arTrackingState.ImageName);
                    }
                });
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}