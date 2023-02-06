using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using ViewSystem.Base;

namespace ViewSystem.Handler
{
	public abstract class BaseViewHandler : IViewHandler
	{
		private readonly Dictionary<Type, IView> _createdViews = new Dictionary<Type, IView>();

		private readonly ViewFactory _viewFactory;
		private readonly List<IView> _views = new List<IView>();

		private IView _activeView;

		protected BaseViewHandler(ViewFactory viewFactory)
		{
			_viewFactory = viewFactory;
		}

		public abstract ViewType ViewType { get; }

		public bool AllViewsAreClosed => _views.Count == 0;

		public bool IsWindowShowing(Type type)
		{
			return _activeView != null && _activeView.GetType() == type ||
				   _views.Count > 0 && _views.Exists(view => view.GetType() == type);
		}

		public TView GetActiveView<TView>() where TView : class, IView
		{
			return _activeView as TView;
		}

		public bool ShowView(Type type)
		{
			var view = GetView(type);

			if (!CanActivateView(view))
				return false;

			ActivateView(view);
			view.Show();
			Log("Shown", view);

			return true;
		}

		public bool ShowViewWithInput<TView, TInput>(TInput input)
				where TInput : IViewInput
				where TView : class, IViewWithInput<TInput>
		{
			return ShowViewWithInput(typeof(TView), input);
		}

		public bool ShowViewWithInput<TInput>(Type type, TInput input) where TInput : IViewInput
		{
			var view = GetView(type) as IViewWithInput<TInput>;

			if (!CanActivateView(view))
				return false;

			ActivateView(view);
			view?.Show(input);
			Log("Shown", view);

			return true;
		}

		public List<string> HideAllViews()
		{
			var hiddenViewNames = new List<string>();

			while (_activeView != null)
			{
				var hiddenViewName = HideActiveView();

				if (!string.IsNullOrEmpty(hiddenViewName))
					hiddenViewNames.Add(hiddenViewName);
			}

			return hiddenViewNames;
		}

		public bool HideView(Type type)
		{
			if (_activeView == null)
				return false;

			if (type == _activeView.GetType())
			{
				HideActiveView();
			}
			else
			{
				var index = _views.FindIndex(view => view.GetType() == type);

				if (index >= 0)
				{
					Log("Hidden", _views[index]);

					_views[index].Hide();
					_views.RemoveAt(index);
				}
				else
				{
					Log("Hidden failed for " + type.Name);
				}
			}

			return true;
		}

		private string HideActiveView()
		{
			if (_activeView == null)
				return string.Empty;

			if (_views.Count > 0)
				_views.Remove(_activeView);

			_activeView.Hide();
			var hiddenViewName = _activeView.GetType().Name;

			Log("Hidden", _activeView);
			ActivateView(_views.LastOrDefault());

			return hiddenViewName;
		}

		public void HideView(IView view)
		{
			HideView(view.GetType());
		}

		private bool CanActivateView(IView view)
		{
			var result = view != null && (_activeView == null || _activeView != view || _activeView.IsShown);

			if (!result)
				Log("can't activate view", view);

			return result;
		}

		private void ActivateView(IView view)
		{
			_activeView = view;

			if (_views.LastOrDefault() != _activeView)
				_views.Add(_activeView);
		}

		private IView GetView(Type type)
		{
			if (_createdViews.ContainsKey(type))
				return _createdViews[type];

			var view = _viewFactory.Spawn<IView>(type);

			_createdViews.Add(type, view);

			return view;
		}

		private void Log(string message, IView view = null)
		{
			// Debug.Log($"[{view}] {message}");
			// new Log()
			// 	   .View(view)
			// 	   .Custom(message)
			// 	   .PrintLog();
		}
	}
}