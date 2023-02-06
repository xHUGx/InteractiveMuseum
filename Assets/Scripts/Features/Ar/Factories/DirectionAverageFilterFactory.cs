using Features.Ar.Configs;
using Features.Ar.Controllers;
using Zenject;

namespace Features.Ar.Factories
{
    public class DirectionAverageFilterFactory : IFactory<Vector3AverageFilter>
    {
        private readonly DiContainer _diContainer;
        private readonly AverageFilterConfig _config;

        public DirectionAverageFilterFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
            _config = _diContainer.ResolveId<AverageFilterConfig>(AverageFilterConfigType.Direction.ToString());
        }

        public Vector3AverageFilter Create()
        {
            return new Vector3AverageFilter(_config);
        }
    }
}