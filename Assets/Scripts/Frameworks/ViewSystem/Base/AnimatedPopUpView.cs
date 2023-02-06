// using Const;
// using Signals;
using UnityEngine;
using ViewSystem.Animation;
using Zenject;

namespace ViewSystem.Base
{
	[Attributes.AttributeViewType(ViewType.Window)]
	public class AnimatedPopUpView : BaseAnimatedView<FadeAndScaleViewAnimation>
	{
#pragma warning disable 0649

		[Header("Animation")] [SerializeField] private CanvasGroup _canvasGroup;

		[SerializeField] private RectTransform _panelRectTransform;

		public override ViewLayer ViewLayer => ViewLayer.PopUp;
		private SignalBus _signalBus;

		[Inject]
		public void Construct(SignalBus signalBus)
		{
			_signalBus = signalBus;
		}

		protected override void InitializeAnimation()
		{
			Animation = new FadeAndScaleViewAnimation(_canvasGroup, _panelRectTransform);
		}

		public override void Show()
		{
			base.Show();
			// _signalBus.Fire(new AudioSignals.PlayAudioRequest(GameConst.SOUND_POPUP_OPEN));
		}

		public override void Hide()
		{
			base.Hide();
			// _signalBus.Fire(new AudioSignals.PlayAudioRequest(GameConst.SOUND_POPUP_CLOSE));
		}
	}
}