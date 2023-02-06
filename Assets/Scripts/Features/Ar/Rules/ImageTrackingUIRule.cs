using System;
using Features.UI.View;
using Frameworks.ViewSystem.Controller;
using Features.App.Controllers;
using Features.App.Data;
using UniRx;
using Zenject;
using Features.Ar.Models;

namespace Features.Ar.Rules
{
    public class ImageTrackingUIRule : IInitializable, IDisposable
    {
        private readonly IViewController _viewController;
        private readonly IAppModel _appModel;

        private readonly  IArTrackingStateProvider _arTrackingState;

        private readonly CompositeDisposable _compositeDisposable;

        public ImageTrackingUIRule(IViewController viewController, IAppModel appModel, IArTrackingStateProvider arTrackingState)
        {
            _viewController = viewController;
            _appModel = appModel;
            _arTrackingState = arTrackingState;


            _compositeDisposable = new CompositeDisposable();
        }

        public void Initialize()
        {

            _appModel
                .GetAppStateAsObservable()
                .Where(value => value.AppState == AppStateType.Localization)
                //value.EventType == StateEventType.Enter)
                .Subscribe(async value =>
                {
                    if (value.EventType == StateEventType.Enter) //if (!_arTrackingState.GetIsTracked())
                    {
                        _viewController.ShowView<SeekingWindow>();
                        return;
                    }

                    if (value.EventType == StateEventType.Exited)
                    {
                        _viewController.HideView<SeekingWindow>();
                    }

                })
                .AddTo(_compositeDisposable);

        }

        
        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable?.Clear();

            _viewController.HideView<SeekingWindow>();
        }
    }
}