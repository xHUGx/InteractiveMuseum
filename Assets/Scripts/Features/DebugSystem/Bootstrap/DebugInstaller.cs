using UnityEngine;
using Utils;
using Zenject;
using Features.DebugSystem.Factories;
using Features.DebugSystem.Messages;
using Features.DebugSystem.Models;
using Features.DebugSystem.Rule;
using Features.DebugSystem.Services;

// ReSharper disable once CheckNamespace
namespace Features.DebugSystem.Config
{
    public class DebugInstaller : Installer<DebugSettings, DebugInstaller>
    {
        private readonly DebugSettings _debugSettings;

        public DebugInstaller(DebugSettings debugSettings)
        {
            _debugSettings = debugSettings;
        }

        public override void InstallBindings()
        {
            Debug.Log("[DebugInstaller] Installing");

            InstallConfigs();
            InstallModels();
            InstallRules();
            InstallSignals();
            InstallFactories();
            InstallServices();
        }

        private void InstallConfigs()
        {
            Container.BindSingleFromInstance(_debugSettings);
        }

        private void InstallModels()
        {
            SRDebug.Init();
            Container.InstallRegistry(SRDebug.Instance);
            
            Container.BindService<DebugGraphicsModel>();
            Container.BindService<DebugArImageInfoEnabledModel>();
        }

        private void InstallRules()
        {
            Container.BindService<ShowDebugImageTrackingWindowRule>();
            Container.BindService<ArDebugGameRule>();
            Container.BindService<FpsDebugRule>();
            Container.BindService<EditorDebugRule>();
            Container.BindService<ScenesDebugGameRule>();
            Container.BindService<GraphicsDebugGameRule>();
        }

        private void InstallSignals()
        {
            Container.DeclareSignal<DebugSignals.SRDebuggerInitialized>();
        }

        private void InstallFactories()
        {
            Container
                .Bind<DebugArImageInfoPresenterFactory>()
                .AsSingle();
        }

        private void InstallServices()
        {
            Container.BindService<DebugArImageTrackingService>();
        }
            
    }
}