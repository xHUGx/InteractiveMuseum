using System;
using Features.App.Controllers;
using Features.App.Data;
using Features.ScenePlayer.Models;
using UniRx;
using Zenject;

// ReSharper disable once CheckNamespace
namespace Features.App.Rules
{
    public class ChangeScenePlayerToLocalizationStateRule : IInitializable, IDisposable
    {
        private readonly IAppModel _appModel;
        private readonly AppController _appController;
        private readonly ScenePlayerModel _scenePlayerModel;

        private readonly CompositeDisposable _compositeDisposable;

        public ChangeScenePlayerToLocalizationStateRule(IAppModel appModel, AppController appController,
            ScenePlayerModel scenePlayerModel)
        {
            _appModel = appModel;
            _appController = appController;
            _scenePlayerModel = scenePlayerModel;

            _compositeDisposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            _scenePlayerModel
                .GetIsPlayingAsObservable()
                .Subscribe(value =>
                {
                    var appState = _appModel.GetAppState();

                    if (appState.AppState == AppStateType.ScenePlayer && appState.EventType == StateEventType.Stay)
                    {
                        if (!value)
                            _appController.SetAppState(AppStateType.Localization);
                    }
                })
                .AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}