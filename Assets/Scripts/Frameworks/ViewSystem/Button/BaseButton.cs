using System;
using View.UI.ButtonCommands;

namespace ViewSystem.Button
{
	public interface IButtonAnimation // todo to move to own script
	{
		void AnimatePress(Action callback);
		void Clear();
	}

	public class BaseButton<TButtonAnimation> : AbstractButtonCommand where TButtonAnimation : IButtonAnimation
	{
		private Action _pressAction;
		protected TButtonAnimation Animation;

		public void BindPressAction(Action action)
		{
			_pressAction = action;
		}

		public void Clear()
		{
			Animation?.Clear();
			_pressAction = null;
		}

		public override void Activate()
		{
			Animation?.AnimatePress(_pressAction);
		}
	}
}