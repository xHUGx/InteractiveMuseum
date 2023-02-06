using System;
using Frameworks.ViewSystem.Controller;
using UniRx;
using UnityEngine;
using ViewSystem.Animation;
using ViewSystem.Attributes;
using Zenject;

namespace ViewSystem.Base
{
	[AttributeViewType(ViewType.Hint)]
	public class BaseHintView : BaseAnimatedView<FadeViewAnimation>
	{
#pragma warning disable 0649
		[Header("Animation")] [SerializeField] private CanvasGroup _canvasGroup;

		public override ViewLayer ViewLayer => ViewLayer.Hint;

		private IViewController _viewController;

		private IDisposable _tapStream;

		[Inject]
		public void Constrtuct(IViewController viewController)
		{
			_viewController = viewController;
		}

		protected override void InitializeAnimation()
		{
			Animation = new FadeViewAnimation(_canvasGroup);
		}

		public override void Show()
		{
			_tapStream?.Dispose();
			_tapStream = Observable.EveryUpdate()
								   .Where(_ => Input.GetMouseButtonUp(0))
								   .First()
								   .Subscribe(_ =>
											  {
												  _tapStream?.Dispose();
												  _viewController.HideView(GetType());
											  });

			base.Show();
		}

		public override void Hide()
		{
			_tapStream?.Dispose();
			base.Hide();
		}
	}
}