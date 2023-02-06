using Features.Ar.Data;
using Features.ImageTracking.Views;
using Zenject;

namespace Features.Ar.Factories
{
    public class ArComponentsViewFactory : IFactory<ArComponentsView>
    {
        private readonly DiContainer _container;

        public ArComponentsViewFactory(DiContainer container)
        {
            _container = container;
        }

        public ArComponentsView Create()
        {
            var newView = _container
                .InstantiatePrefabResourceForComponent<ArComponentsView>(ArViewResources.ArComponents);

            return newView;
        }
    }
}