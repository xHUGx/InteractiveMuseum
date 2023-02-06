using Features.UI.View;
using Features.App.Data;
using Features.DebugSystem.Config;
using UnityEngine;
using ViewSystem;
using Zenject;

// ReSharper disable once CheckNamespace
namespace Bootstrap
{
    public class InitialInstaller : Installer
    {
        public override void InstallBindings()
        {
            InstallMisc();
            BindSignals();
            BindServices();
            InstallDebug();
            InstallViews();
        }

        private void InstallDebug()
        {
            var debugSettings = Resources.Load<DebugSettings>("DebugSettings");

            if (debugSettings != null && debugSettings.IsDebugSettingsEnabled)
            {
                Debug.Log("[InitialInstaller] Install debug!");
                DebugInstaller.Install(Container, debugSettings);
            }
        }

        private void InstallMisc()
        {
            Container.Install<ViewInstaller>();
        }

        private void InstallViews()
        {
            Container
                .Bind<UiView>()
                .FromComponentInNewPrefabResource(ViewResources.UI)
                .AsCached()
                .NonLazy();
        }

        private void BindSignals()
        {
            SignalBusInstaller.Install(Container);
        }

        private void BindServices()
        {
        }
    }
}