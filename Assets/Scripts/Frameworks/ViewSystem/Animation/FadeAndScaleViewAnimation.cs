using System;
using DG.Tweening;
using UnityEngine;

namespace ViewSystem.Animation
{
	public class FadeAndScaleViewAnimation : IViewAnimation
	{
		private readonly CanvasGroup _canvasGroup;
		private readonly RectTransform _panelRectTransform;
		private readonly Settings _settings;


		private Tween _tweenHide;
		private Tween _tweenShow;

		public FadeAndScaleViewAnimation(CanvasGroup canvasGroup, RectTransform panelRectTransform,
										 Settings settings = null)
		{
			_canvasGroup = canvasGroup;
			_panelRectTransform = panelRectTransform;
			_settings = settings ?? Settings.Default;
		}

		public bool IsHiding => _tweenHide != null && _tweenHide.IsActive() && _tweenHide.IsPlaying();

		public void AnimateShow(Action callback)
		{
			Clear();

			_panelRectTransform.localScale = Vector3.one * _settings.Scale;
			_canvasGroup.alpha = _settings.HideFade;

			_tweenShow = DOTween.Sequence()
								.Append(_canvasGroup.DOFade(_settings.ShowFade, _settings.ShowingTime)
													.SetEase(Ease.OutCirc))
								.Join(_panelRectTransform
									 .DOScale(_settings.TargetScale, _settings.ShowingTime).SetEase(Ease.InOutBack))
								.AppendCallback(() => callback?.Invoke());
		}

		public void AnimateHide(Action callback)
		{
			Clear();

			_tweenHide = DOTween.Sequence()
								.Append(_canvasGroup.DOFade(_settings.HideFade, _settings.HidingTime)
													.SetEase(Ease.InQuart))
								.Join(_panelRectTransform.DOScale(_settings.Scale, _settings.HidingTime)
														 .SetEase(Ease.InBack, _settings.Overshoot))
								.AppendCallback(() => callback?.Invoke());
		}

		public void Clear()
		{
			_tweenHide?.Kill();
			_tweenShow?.Kill();
		}

		public class Settings
		{
			public float Scale { get; set; } = 0.97f;
			public float ShowFade { get; set; } = 1f;
			public float HideFade { get; set; } = 0f;
			public float TargetScale { get; set; } = 1f;
			public float ShowingTime { get; set; } = 0.4f;
			public float HidingTime { get; set; } = 0.3f;
			public float Overshoot { get; set; } = 2f;

			public static Settings Default => new Settings();
		}
	}
}