using Features.Ar.Configs;
using Features.Ar.Controllers;
using Zenject;

namespace Features.Ar.Factories
{
    public class PositionAverageFilterFactory : IFactory<Vector3AverageFilter>
    {
        private readonly DiContainer _diContainer;
        private readonly AverageFilterConfig _config;

        public PositionAverageFilterFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
            _config = _diContainer.ResolveId<AverageFilterConfig>(AverageFilterConfigType.Position.ToString());
        }

        public Vector3AverageFilter Create()
        {
            return new Vector3AverageFilter(_config);
        }
    }
}