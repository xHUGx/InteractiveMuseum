using System;
using DG.Tweening;
using UnityEngine;

namespace ViewSystem.Button
{
	public class ButtonScaleAnimation : IButtonAnimation // todo to move to own script
	{
		private const float TagetScale = 1.1f;
		private const float DefaultScale = 1f;
		private readonly Transform _targetTransform;

		private Tween _tweenHide;

		public ButtonScaleAnimation(Transform targetTransform)
		{
			_targetTransform = targetTransform;
		}

		public void AnimatePress(Action callback)
		{
			Clear();

			_tweenHide = DOTween.Sequence()
								.Append(_targetTransform
									   .DOScale(_targetTransform.localScale, TagetScale).SetEase(Ease.InOutBack))
								.Append(_targetTransform
									   .DOScale(_targetTransform.localScale, DefaultScale).SetEase(Ease.InOutBack))
								.AppendCallback(() => callback?.Invoke());
		}

		public void Clear()
		{
			_targetTransform.localScale = Vector3.one;

			_tweenHide?.Kill();
		}
	}

	public class ScaleButton : BaseButton<ButtonScaleAnimation>
	{
#pragma warning disable 0649
		[SerializeField] private Transform _animationTransform;

		protected override void Awake()
		{
			base.Awake();
			Animation = new ButtonScaleAnimation(_animationTransform);
		}
	}
}