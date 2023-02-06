using Features.App.Data;
using Features.App.Controllers.AppStates;
using Zenject;

namespace Features.App.Factories
{
    public class AppStatePlaceholderFactory : PlaceholderFactory<IAppState>
    {
    }

    public class AppStatesFactory : IFactory<AppStateType, IAppState>
    {
        private readonly DiContainer _container;

        public AppStatesFactory(DiContainer container)
        {
            _container = container;
        }

        public IAppState Create(AppStateType appStateType)
        {
            var factory = _container.ResolveId<AppStatePlaceholderFactory>(appStateType.ToString());
            return factory.Create();
        }
    }
}