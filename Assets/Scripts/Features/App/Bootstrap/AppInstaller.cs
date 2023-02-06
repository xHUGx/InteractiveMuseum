using Features.App.Controllers;
using Features.App.Data;
using Features.App.Factories;
using Features.App.Controllers.AppStates;
using Features.SceneManagement.Services;
using Features.App.Rules;
using UnityEngine;
using Utils;
using ViewSystem;
using Zenject;

// ReSharper disable once CheckNamespace
namespace Features.App.Bootstrap
{
    public class AppInstaller : MonoInstaller
    {
        private DiContainer _container;

        public override void InstallBindings()
        {
            Debug.Log("[AppInstaller] Installing");

            _container = ProjectContext.Instance.Container;

            RunInstallers();

            InstallModels();
            InstallRules();

            InstallFactories();
        }

        private void RunInstallers()
        {
            
        }


        private void InstallModels()
        {
            _container.BindService<AppModel>();
            _container.BindService<AppController>();
            _container.BindService<AppViewController>();

            _container.BindService<ScreenSleepController>();
        }

        private void InstallRules()
        {
            _container.BindInterfacesTo<AppLoadingRule>().AsSingle();
            _container.BindInterfacesTo<SceneLoaderService>().AsSingle();

            _container.BindService<ChangeScenePlayerToLocalizationStateRule>();
            _container.BindService<ChangeLocalizationToScenePlayerStateRule>();
        }

        private void InstallFactories()
        {
            _container.Bind<AppStatesFactory>().AsSingle();

            InstallAppStateFactory<IdleState>(AppStateType.Idle);
            InstallAppStateFactory<LoadingState>(AppStateType.Loading);
            InstallAppStateFactory<LocalizationState>(AppStateType.Localization);
            InstallAppStateFactory<ScenePlayerState>(AppStateType.ScenePlayer);
        }

        private void InstallAppStateFactory<TState>(AppStateType appStateType) where TState : IAppState
        {
            _container
                .BindFactory<IAppState, AppStatePlaceholderFactory>()
                .WithId(appStateType.ToString())
                .To<TState>();
        }
    }
}