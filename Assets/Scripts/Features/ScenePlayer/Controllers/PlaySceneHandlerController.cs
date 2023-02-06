using Features.ScenePlayer.Handlers;
using Features.ScenePlayer.Models;
using System;
using UniRx;
using Zenject;

namespace Features.ScenePlayer.Controllers
{
    public class PlaySceneHandlerController : IInitializable, IDisposable
    {
        private readonly ScenePlayerModel _scenePlayerModel;
        private readonly PlaySceneHandlerFactory _playSceneHandlerFactory;

        private IDisposable _disposable;
        private IPlaySceneHandler _playSceneHandler;

        public PlaySceneHandlerController(ScenePlayerModel scenePlayerModel, PlaySceneHandlerFactory playSceneHandlerFactory)
        {
            _scenePlayerModel = scenePlayerModel;
            _playSceneHandlerFactory = playSceneHandlerFactory;
        }

        public void Initialize()
        {
            _disposable = _scenePlayerModel.GetIsPlayingAsObservable()
                .Subscribe(value =>
                {
                    if (value)
                    {
                        _playSceneHandler = _playSceneHandlerFactory.Spawn(_scenePlayerModel.SceneName);
                    }
                    else if (_playSceneHandler != null)
                    {
                        _playSceneHandler.Dispose();
                        _playSceneHandler = null;
                    }
                });
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}