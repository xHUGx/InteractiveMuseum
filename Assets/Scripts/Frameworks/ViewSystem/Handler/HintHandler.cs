namespace ViewSystem.Handler
{
	public class HintHandler : BaseViewHandler
	{
		public HintHandler(ViewFactory viewFactory) : base(viewFactory)
		{
		}

		public override ViewType ViewType => ViewType.Hint;
	}
}