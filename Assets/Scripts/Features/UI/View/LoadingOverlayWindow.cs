using UnityEngine;
using ViewSystem;
using ViewSystem.Animation;
using ViewSystem.Attributes;
using ViewSystem.Base;
using Zenject;

namespace Features.UI.View
{
    [AttributeViewType(ViewType.Window)]
    public class LoadingOverlayWindow: BaseAnimatedView<FadeViewAnimation>
    {
        [Header("Animation")] [SerializeField] private CanvasGroup canvasGroup;

        public override ViewLayer ViewLayer => ViewLayer.Global;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void InitializeAnimation()
        {
            Animation = new FastFadeViewAnimation(canvasGroup);
        }

    }
}