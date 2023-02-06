namespace ViewSystem.Handler
{
	public class WindowHandler : BaseViewHandler
	{
		public WindowHandler(ViewFactory viewFactory) : base(viewFactory)
		{
		}

		public override ViewType ViewType => ViewType.Window;
	}
}