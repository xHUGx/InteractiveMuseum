using ViewSystem.Animation;

namespace ViewSystem.Base
{
	public abstract class BaseAnimatedView<TViewAnimation> : BaseView, IAnimatedView
			where TViewAnimation : IViewAnimation
	{
		protected TViewAnimation Animation;

		public override void Show()
		{
			base.Show();

			if (Animation != null)
				Animation.AnimateShow(OnEndAnimatingShow);
		}

		public override void Hide()
		{
			if (Animation != null)
				Animation.AnimateHide(OnEndAnimatingHide);
			else
				OnEndAnimatingHide();
		}

		protected abstract void InitializeAnimation();

		protected virtual void Awake()
		{
			InitializeAnimation();
		}

		protected virtual void OnEndAnimatingShow()
		{
		}

		protected virtual void OnEndAnimatingHide()
		{
			base.Hide();
		}

		protected virtual void OnDestroy()
		{
			Animation?.Clear();
		}
	}
}