using System;
using Features.UI.View;
using Frameworks.ViewSystem.Controller;
using Features.App.Data;
using UniRx;
using Zenject;

namespace Features.App.Controllers
{
    public class AppViewController : IInitializable, IDisposable
    {
        private readonly IViewController _viewController;
        private readonly AppModel _appModel;
        private readonly CompositeDisposable _compositeDisposable;

        public AppViewController(IViewController viewController, AppModel appModel)
        {
            _viewController = viewController;
            _appModel = appModel;

            _compositeDisposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            _appModel
                .GetAppStateAsObservable()
                .Subscribe(state =>
                {
                    switch (state.AppState)
                    {
                        case AppStateType.Idle:
                            if (state.EventType == StateEventType.Enter)
                            {
                                _viewController.ShowView<LoadingOverlayWindow>();
                            }
                            break;
                        case AppStateType.Loading:
                            switch (state.EventType)
                            {
                                case (StateEventType.Enter):
                                    _viewController.ShowView<LoadingOverlayWindow>();
                                    break;
                                case (StateEventType.Exited):
                                    _viewController.HideView<LoadingOverlayWindow>();
                                    break;
                            }
                            break;
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