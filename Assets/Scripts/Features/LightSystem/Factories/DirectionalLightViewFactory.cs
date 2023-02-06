using Zenject;
using Features.LightSystem.Data;
using Features.LightSystem.Views;

namespace Features.LightSystem.Factories
{
    public class DirectionalLightViewFactory : IFactory<DirectionalLightView>
    {
        private readonly DiContainer _container;

        public DirectionalLightViewFactory(DiContainer container)
        {
            _container = container;
        }

        public DirectionalLightView Create()
        {
            var newView = _container
                .InstantiatePrefabResourceForComponent<DirectionalLightView>(LightViewResources.DirectionalLight);

            return newView;
        }
    }
}