using Features.DebugSystem.Views;
using Features.DebugSystem.Data;
using UnityEngine;
using Zenject;

namespace Features.DebugSystem.Factories
{
    public class DebugArImageInfoPresenterFactory : IFactory<Transform, DebugArImageInfoPresenter>
    {
        private readonly DiContainer _container;

        public DebugArImageInfoPresenterFactory(DiContainer container)
        {
            _container = container;
        }

        public DebugArImageInfoPresenter Create(Transform parent)
        {
            var newPresenter = _container
                .InstantiatePrefabResourceForComponent<DebugArImageInfoPresenter>(DebugResources.DebugArImageInfo, parent);

            newPresenter.Initialize();
            
            return newPresenter;
        }
    }
}