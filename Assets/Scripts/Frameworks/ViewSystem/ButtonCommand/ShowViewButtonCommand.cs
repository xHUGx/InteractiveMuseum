using System;
using Frameworks.ViewSystem.Controller;
using UnityEngine;
using View.UI.ButtonCommands;
using Zenject;

namespace ViewSystem.ButtonCommand
{
	public class ShowViewButtonCommand : AbstractButtonCommand
	{
#pragma warning disable 0649
#if UNITY_EDITOR
		[Attributes.AttributeViewName]
#endif
		[SerializeField]
		private string _viewType;

		[SerializeField] private bool _secondTapCloseWindow;
		[SerializeField] private bool _hideAllPreviousWindows = true;
		[SerializeField] private bool _showOnAwake;

		private IViewController _viewController;

		[Inject]
		public void Construct(IViewController viewController)
		{
			_viewController = viewController;

			if (_showOnAwake)
				Activate();
		}

		protected override void Awake()
		{
			if (string.IsNullOrEmpty(_viewType))
				// Debug.LogError($"[ShowViewButtonCommand] view type {_viewType} can't be null or empty!");

			base.Awake();
		}

		public override void Activate()
		{
			if (string.IsNullOrEmpty(_viewType))
			{
				// Debug.LogError("[ShowViewButtonCommand] view type {_viewType} can't be null or empty!");
				return;
			}

			var type = Type.GetType(_viewType);

			if (_viewController == null)
			{
				// Debug.LogError("[ShowViewButtonCommand] can't find ViewController!");
				return;
			}

			if (type == null)
			{
				// Debug.LogError("[ShowViewButtonCommand] type can't be null!");
				return;
			}

			if (_viewController.IsViewShowing(type))
			{
				if (_secondTapCloseWindow)
					_viewController.HideView(type);
			}
			else
			{
				if (_hideAllPreviousWindows)
					_viewController.HideAllByType(ViewType.Window);

				_viewController.ShowView(type);
			}
		}
	}
}