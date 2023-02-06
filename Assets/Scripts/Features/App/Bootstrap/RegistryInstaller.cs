using Features.Ar.Configs;
using Features.SceneManagement.Config;
using UnityEngine;
using Utils;
using Zenject;

namespace Features.App.Bootstrap
{
    public class RegistryInstaller : MonoInstaller
    {
        [SerializeField] private SceneRegistry _sceneRegistry;
        [SerializeField] private ArImageTrackingConfig _arImageTrackingConfig;
        [SerializeField] private AverageFilterConfig _positionAverageFilterConfig;
        [SerializeField] private AverageFilterConfig _directionAverageFilterConfig;

        public override void InstallBindings()
        {
            Container.InstallRegistry(_sceneRegistry);
            Container.InstallRegistry(_arImageTrackingConfig);

            Container.BindInstance(_positionAverageFilterConfig)
                .WithId(AverageFilterConfigType.Position.ToString());

            Container.BindInstance(_directionAverageFilterConfig)
                .WithId(AverageFilterConfigType.Direction.ToString());
        }
    }
}