using UnityEngine;

namespace ViewSystem.Animation
{
	public class FastFadeViewAnimation : FadeViewAnimation
	{
		public FastFadeViewAnimation(CanvasGroup canvasGroup) : base(canvasGroup)
		{
		}

		protected override float ShowingTime => 0.2f;
		protected override float HidingTime => 0.2f;
	}
}