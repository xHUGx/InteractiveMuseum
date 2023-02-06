using Frameworks.ViewSystem.Controller;
using View.UI.ButtonCommands;
using Zenject;

namespace ViewSystem.ButtonCommand
{
	public class HideAllHintsButtonCommand : AbstractButtonCommand
	{
		private IViewController _viewController;

		[Inject]
		public void Construct(IViewController viewController)
		{
			_viewController = viewController;
		}

		public override void Activate()
		{
			_viewController.HideAllByType(ViewType.Hint);
		}
	}
}