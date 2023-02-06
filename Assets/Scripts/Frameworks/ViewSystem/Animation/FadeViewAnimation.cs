using System;
using DG.Tweening;
using UnityEngine;

namespace ViewSystem.Animation
{
	public class FadeViewAnimation : IViewAnimation
	{
		private const float ShowFade = 1f;
		private const float HideFade = 0f;

		private readonly CanvasGroup _canvasGroup;
		private Tween _tweenHide;

		private Tween _tweenShow;

		public FadeViewAnimation(CanvasGroup canvasGroup)
		{
			_canvasGroup = canvasGroup;
		}

		public bool IsHiding => _tweenHide != null && _tweenHide.IsActive() && _tweenHide.IsPlaying();
		protected virtual float ShowingTime { get; } = 0.5f;
		protected virtual float HidingTime { get; } = 0.5f;

		public void AnimateShow(Action callback)
		{
			Clear();

			_tweenShow = DOTween.Sequence()
								.Append(_canvasGroup.DOFade(ShowFade, ShowingTime).SetEase(Ease.OutCubic))
								.AppendCallback(() => callback?.Invoke());
		}

		public void AnimateHide(Action callback)
		{
			Clear();

			_tweenHide = DOTween.Sequence()
								.Append(_canvasGroup.DOFade(HideFade, HidingTime).SetEase(Ease.OutCubic))
								.AppendCallback(() => callback?.Invoke());
		}

		public void Clear()
		{
			_tweenHide?.Kill();
			_tweenShow?.Kill();
		}
	}
}