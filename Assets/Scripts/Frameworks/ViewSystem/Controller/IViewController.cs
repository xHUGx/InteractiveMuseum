using System;
using ViewSystem;
using ViewSystem.Base;

namespace Frameworks.ViewSystem.Controller
{
	public interface IViewController
	{
		void ShowView(Type type);

		void ShowView<TView>()
				where TView : class, IView;

		void ShowView<TView, TInput>(TInput input)
				where TInput : IViewInput
				where TView : class, IViewWithInput<TInput>;

		void ShowView<TInput>(Type type, TInput input)
				where TInput : IViewInput;

		bool IsViewShowing(Type type);
		bool AllViewsAreClosed(params ViewType[] viewTypes);

		TView GetActiveView<TView>()
				where TView : class, IView;

		void HideAll();
		void HideAllByType(params ViewType[] viewTypes);
		void HideView(IView view);

		void HideView<TView>()
				where TView : class, IView;

		void HideView(Type type);
	}
}