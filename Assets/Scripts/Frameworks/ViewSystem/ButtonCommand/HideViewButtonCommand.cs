using Frameworks.ViewSystem.Controller;
using UnityEngine;
using View.UI.ButtonCommands;
using ViewSystem.Base;
using Zenject;

namespace ViewSystem.ButtonCommand
{
	public class HideViewButtonCommand : AbstractButtonCommand
	{
#pragma warning disable 0649
		[SerializeField] private bool _ignoreLocking;

		private IViewController _viewController;

		protected override bool IgnoreLocking => _ignoreLocking;

		[Inject]
		public void Construct(IViewController viewController)
		{
			_viewController = viewController;
		}

		public override void Activate()
		{
			var view = GetComponentInParent<IView>();

			if (view != null)
				_viewController.HideView(view);
		}
	}
}