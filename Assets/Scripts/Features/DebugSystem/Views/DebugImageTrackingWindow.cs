using UnityEngine;
using ViewSystem;
using ViewSystem.Animation;
using ViewSystem.Base;
using Zenject;
using UniRx;
using ViewSystem.Attributes;
using System;
using Features.App.Controllers;
using Features.App.Data;

namespace Features.UI.View
{
    [AttributeViewType(ViewType.Window)]
    public class DebugImageTrackingWindow : BaseAnimatedView<FadeViewAnimation>
    {
        [Header("Animation")] [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private GameObject _buttonFindImage;
        [SerializeField] private GameObject _buttonLostImage;

        [SerializeField] private GameObject _locker;

        public override ViewLayer ViewLayer => ViewLayer.Window;

        private AppModel _appModel;

        private IDisposable _appStateStream;

        [Inject]
        public void Construct(AppModel appModel)
        {
            _appModel = appModel;
        }

        protected override void InitializeAnimation()
        {
            Animation = new FastFadeViewAnimation(canvasGroup);
        }

        public override void Show()
        {
            base.Show();

            CheckTrackingState();

            _appStateStream = _appModel
                .GetAppStateAsObservable()
                .Subscribe(state =>
                {
                    _locker.SetActive(state.EventType != StateEventType.Stay);

                    if (state.EventType == StateEventType.Stay)
                    {
                        CheckTrackingState();
                    }
                });
        }

        public override void Hide()
        {
            base.Hide();

            _appStateStream?.Dispose();
        }

        private void CheckTrackingState()
        {
            var imageTrackingState = _appModel.GetAppState().AppState == AppStateType.Localization;

            _buttonFindImage.SetActive(imageTrackingState);
            _buttonLostImage.SetActive(!imageTrackingState);
        }

    }
}