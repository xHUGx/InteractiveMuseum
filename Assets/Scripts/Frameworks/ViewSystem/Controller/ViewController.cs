using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Utils;
using ViewSystem;
using ViewSystem.Attributes;
using ViewSystem.Base;
using ViewSystem.Handler;
using Zenject;

namespace Frameworks.ViewSystem.Controller
{
	public class ViewController : IViewController
	{
		private readonly Dictionary<ViewType, IViewHandler> _handlers;
		private readonly SignalBus _signalBus;

		public ViewController(SignalBus signalBus, DiContainer diContainer)
		{
			_signalBus = signalBus;
			_handlers = diContainer.ResolveAll<IViewHandler>()
								   .ToDictionary(h => h.ViewType, h => h);
		}

		public void ShowView(Type type)
		{
			var handler = GetHandler(type);
			var result = handler?.ShowView(type);

			FireSignal(result, new ViewSignals.Shown(type.Name));
		}

		public void ShowView<TView>() where TView : class, IView
		{
			Debug.Log("#TryShow: " + typeof(TView));

			ShowView(typeof(TView));
		}

		public void ShowView<TView, TInput>(TInput input) where TView : class, IViewWithInput<TInput>
														  where TInput : IViewInput
		{
			ShowView(typeof(TView), input);
		}

		public void ShowView<TInput>(Type type, TInput input) where TInput : IViewInput
		{
			var result = GetHandler(type)?.ShowViewWithInput(type, input);
			FireSignal(result, new ViewSignals.Shown(type.Name));
		}

		public bool IsViewShowing(Type type)
		{
			var handler = GetHandler(type);
			if (handler == null)
				return false;

			return handler.IsWindowShowing(type);
		}

		public bool AllViewsAreClosed(params ViewType[] viewTypes)
		{
			foreach (var viewType in viewTypes)
			{
				var handler = GetHandler(viewType);

				if (handler == null || !handler.AllViewsAreClosed)
					return false;
			}

			return true;
		}

		public TView GetActiveView<TView>() where TView : class, IView
		{
			return GetHandler(typeof(TView))?.GetActiveView<TView>();
		}

		public void HideAll()
		{
			HideAllByType(ViewType.Hint, ViewType.Tutorial, ViewType.Window);
		}

		public void HideAllByType(params ViewType[] viewTypes)
		{
			var closedViewNames = new List<string>();

			foreach (var handler in _handlers)
			{
				if (!viewTypes.Contains(handler.Key))
					continue;

				var viewNames = handler.Value.HideAllViews();
				closedViewNames.AddRange(viewNames);
			}

			for (var i = 0; i < closedViewNames.Count; i++)
			{
				var viewName = closedViewNames[i];

				FireSignal(true, new ViewSignals.Hidden(viewName, i == closedViewNames.Count - 1));
			}
		}

		public void HideView(IView view)
		{
			HideView(view.GetType());
		}

		public void HideView<TView>() where TView : class, IView
		{
			HideView(typeof(TView));
		}

		public void HideView(Type type)
		{
			var handler = GetHandler(type);
			var result = handler?.HideView(type);

			FireSignal(result, new ViewSignals.Hidden(type.Name, AllViewsAreClosed(ViewType.Window)));
		}

		private void FireSignal<TSignal>(bool? result, TSignal signal) where TSignal : ViewSignals.BaseActivitySignal
		{
			if (result == null || !result.Value)
				return;

			_signalBus.Fire(signal);
		}

		private IViewHandler GetHandler(Type type)
		{
			var attribute = (AttributeViewType) type.GetCustomAttribute(typeof(AttributeViewType));
			if (attribute == null)
			{
				// Debug.LogError($"[ViewController] type <{type.Name}> doesn't have attribute AttributeViewType");
				// new Log()
				// 	   .View(null)
				// 	   .Custom($"type <{type.Name}> doesn't have attribute AttributeViewType")
				// 	   .PrintError(true);
				return null;
			}

			return GetHandler(attribute.ViewType);
		}

		private IViewHandler GetHandler(ViewType viewType)
		{
			if (!_handlers.ContainsKey(viewType))
			{
				// Debug.LogError($"There is no handler for <{viewType}>!");
				// new Log()
				// 	   .View(null)
				// 	   .Custom($"There is no hadler for <{viewType}>!")
				// 	   .PrintError(true);
				return null;
			}

			return _handlers[viewType];
		}
	}
}