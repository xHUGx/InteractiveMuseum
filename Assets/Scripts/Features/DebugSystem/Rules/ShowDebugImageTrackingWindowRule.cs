using Features.App.Controllers;
using Features.DebugSystem.Config;
using Features.UI.View;
using Frameworks.ViewSystem.Controller;
using System;
using Zenject;
using UniRx;

namespace Features.DebugSystem.Rule
{
    public class ShowDebugImageTrackingWindowRule : IInitializable, IDisposable
    {

        private readonly AppModel _appModel;

        private readonly IViewController _viewController;
        private readonly DebugSettings _debugSettings;

        private IDisposable _disposable;

        public ShowDebugImageTrackingWindowRule([InjectOptional] DebugSettings debugSettings,
            AppModel appModel, IViewController viewController)
        {
            _viewController = viewController;
            _debugSettings = debugSettings;
            _appModel = appModel;
        }

        public void Initialize()
        {
            _disposable = _appModel.GetAppStateAsObservable()
                .Where(state => state.AppState == App.Data.AppStateType.Loading && state.EventType == StateEventType.Exited)
                .Subscribe(state =>
            {
                if (_debugSettings != null && _debugSettings.ImageTrackingDebug
                    && !_viewController.IsViewShowing(typeof(DebugImageTrackingWindow)))
                {
                    _viewController.ShowView<DebugImageTrackingWindow>();
                }
            });
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}