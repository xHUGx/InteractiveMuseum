using System;
using System.Collections.Generic;
using System.Linq;
using Frameworks.ViewSystem.Data;
using UnityEngine;
using ViewSystem.Base;
using Zenject;

namespace ViewSystem
{
	public class ViewFactory
	{
		private readonly DiContainer _container;

		private readonly Dictionary<ViewLayer, Transform> _holders = new Dictionary<ViewLayer, Transform>();
		private readonly BaseView[] _viewPrefabs;

		public ViewFactory(DiContainer container, SignalBus signalBus)
		{
			_container = container;
			_viewPrefabs = Resources.LoadAll<BaseView>(ViewSystemResources.RESOURCES_FOLDER_WINDOWS_PREFABS);
			signalBus.Subscribe<ViewSignals.AddHolder>(signal => AddHolder(signal.Transform, signal.ViewLayer));
		}

		public TView Spawn<TView>(Type type) where TView : class, IView
		{
			var prefab = _viewPrefabs.FirstOrDefault(w => w.GetType().Name.Equals(type.Name));
			if (prefab == null)
			{
				// Debug.LogWarning("[ViewFactory] can't find view for type : " + type);
				return null;
			}

			var view = _container.InstantiatePrefabForComponent<TView>(prefab.gameObject);

			if (view == null)
			{
				// Debug.LogError("There is no view with type " + type.Name);
				return null;
			}

			if (!_holders.ContainsKey(view.ViewLayer))
			{
				// Debug.LogError($"Holder {view.ViewLayer} for {view.GetType().Name} was not found");
			}

			view.Initialize(_holders[view.ViewLayer]);

			return view;
		}

		private void AddHolder(Transform transform, ViewLayer viewLayer)
		{
			if (!_holders.ContainsKey(viewLayer))
				_holders.Add(viewLayer, null);

			_holders[viewLayer] = transform;
		}
	}
}