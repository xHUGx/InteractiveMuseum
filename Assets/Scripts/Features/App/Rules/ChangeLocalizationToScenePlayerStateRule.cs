using System;
using Features.App.Controllers;
using Features.App.Data;
using Features.Ar.Models;
using UniRx;
using Zenject;

namespace Features.App.Rules
{
    public class ChangeLocalizationToScenePlayerStateRule : IInitializable, IDisposable
    {
        private readonly IAppModel _appModel;
        private readonly AppController _appController;
        private readonly IArTrackingStateProvider _arTrackingState;

        private readonly CompositeDisposable _compositeDisposable;

        public ChangeLocalizationToScenePlayerStateRule(IAppModel appModel, AppController appController,
            IArTrackingStateProvider arTrackingState)
        {
            _appModel = appModel;
            _appController = appController;
            _arTrackingState = arTrackingState;

            _compositeDisposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            _arTrackingState
                .GetIsTrackedAsObservable()
                .Subscribe(value =>
                {
                    var appState = _appModel.GetAppState();

                    if (appState.AppState == AppStateType.Localization && appState.EventType == StateEventType.Stay)
                    {
                        if (value)
                            _appController.SetAppState(AppStateType.ScenePlayer);
                    }
                })
                .AddTo(_compositeDisposable);

            _appModel
                .GetAppStateAsObservable()
                .Where(appState =>
                    (appState.AppState == AppStateType.Localization && appState.EventType == StateEventType.Stay))
                .Subscribe(value =>
                {
                    if (_arTrackingState.GetIsTracked())
                    {
                        _appController.SetAppState(AppStateType.ScenePlayer);
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