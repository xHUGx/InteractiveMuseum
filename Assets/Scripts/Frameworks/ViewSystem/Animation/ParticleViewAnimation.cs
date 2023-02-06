using System;
using DG.Tweening;
using UnityEngine;

namespace ViewSystem.Animation
{
	public class ParticleViewAnimation : IViewAnimation
	{
		private const int Turns = 4;
		private const float ShowDuration = 1f;
		private const float HideDuration = 0.5f;

		private readonly RectTransform _panelRectTransform;
		private readonly ParticleSystem _particleSystem;

		private readonly Vector3 _startPosition = new Vector3(0, -2500f, 0);
		private readonly Vector3 _visiblePosition = new Vector3(0, 0, 0);

		private Tween _tweenHide;
		private Tween _tweenShow;

		public ParticleViewAnimation(
				RectTransform panelRectTransform,
				ParticleSystem particleSystem)
		{
			_panelRectTransform = panelRectTransform;
			_particleSystem = particleSystem;
		}

		public bool IsHiding => _tweenHide != null && _tweenHide.IsActive() && _tweenHide.IsPlaying();

		public void AnimateShow(Action callback)
		{
			Clear();

			_panelRectTransform.anchoredPosition3D = _startPosition;
			_panelRectTransform.localRotation = Quaternion.identity;

			_tweenShow = DOTween.Sequence()
								.AppendCallback(() => _particleSystem.Play())
								.Join(_panelRectTransform
									 .DOLocalMove(_visiblePosition, ShowDuration).SetEase(Ease.OutSine))
								.Join(_panelRectTransform.DOLocalRotate(new Vector3(0, 180f, 0), ShowDuration / Turns)
														 .SetEase(Ease.Linear)
														 .SetLoops(Turns, LoopType.Yoyo)).SetEase(Ease.OutCubic, 5f)
								.AppendCallback(() => callback?.Invoke());
		}

		public void AnimateHide(Action callback)
		{
			Clear();

			_panelRectTransform.localRotation = Quaternion.identity;

			_tweenHide = _tweenHide = DOTween.Sequence()
											 .AppendCallback(() => _particleSystem.Stop())
											 .Append(_panelRectTransform
													.DOLocalMove(_startPosition, HideDuration).SetEase(Ease.InBack))
											 .AppendCallback(() => callback?.Invoke());
		}

		public void Clear()
		{
			_tweenHide?.Kill();
			_tweenShow?.Kill();
		}
	}
}