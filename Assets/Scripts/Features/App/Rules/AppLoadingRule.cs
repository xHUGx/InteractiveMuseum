using System;
using Features.App.Controllers;
using Features.App.Data;
using UniRx;
using Zenject;

// ReSharper disable once CheckNamespace
namespace Features.App.Rules
{
    public class AppLoadingRule : IInitializable, IDisposable
    {
        private readonly IAppModel _appModel;
        private readonly AppController _appController;

        private readonly CompositeDisposable _compositeDisposable;

        public AppLoadingRule(IAppModel appModel, AppController appController)
        {
            _appModel = appModel;
            _appController = appController;

            _compositeDisposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            _appModel
                .GetAppStateAsObservable()
                .Where(value => value.AppState == AppStateType.Loading && value.EventType == StateEventType.Stay)
                .Subscribe(appState =>
                {
                    _appController.SetAppState(AppStateType.Localization);
                })
                .AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}