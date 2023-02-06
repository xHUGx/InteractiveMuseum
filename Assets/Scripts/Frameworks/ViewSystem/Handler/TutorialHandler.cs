namespace ViewSystem.Handler
{
	public class TutorialHandler : BaseViewHandler
	{
		public TutorialHandler(ViewFactory viewFactory) : base(viewFactory)
		{
		}

		public override ViewType ViewType => ViewType.Tutorial;
	}
}