using UnityEngine;
using Utils;
using Zenject;
using Features.LightSystem.Factories;
using Features.LightSystem.Messages;
using Features.LightSystem.Services;

namespace Features.LightSystem.Bootstrap
{
    public class LightInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("[LightInstaller] Installing");

            InstallModels();
            InstallRules();
            InstallSignals();
            InstallFactories();
            InstallServices();
        }

        private void InstallModels()
        {
            // Container.BindService<>();
        }

        private void InstallRules()
        {
            // Container.BindRule<>();
        }

        private void InstallSignals()
        {
            Container.DeclareSignal<LightSignals.RegisterNewDirectionalLightHandler>();
        }

        private void InstallFactories()
        {
            Container
                .Bind<DirectionalLightViewFactory>()
                .AsSingle();
        }

        private void InstallServices()
        {
            Container.BindService<LightHandlerService>();
            Container.BindService<DirectionalLightService>();
        }
    }
}