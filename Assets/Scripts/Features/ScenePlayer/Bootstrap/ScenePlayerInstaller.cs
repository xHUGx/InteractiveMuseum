using Features.ScenePlayer.Controllers;
using Features.ScenePlayer.Models;
using Features.ScenePlayer.Rules;
using Features.ScenePlayer.Handlers;
using UnityEngine;
using Utils;
using Zenject;
using Pool;
using Features.ScenePlayer.Data;
using Features.ScenePlayer.Messages;

// ReSharper disable once CheckNamespace
namespace Features.ScenePlayer.Bootstrap
{
    public class ScenePlayerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("[ScenePlayerInstaller] Installing");

            InstallModels();
            InstallRules();
            InstallSignals();
            InstallFactories();
            InstallHandlers();
        }

        private void InstallModels()
        {
            Container.BindService<ScenePlayerModel>();
            Container.BindService<ScenePlayerComponentsController>();
            Container.BindService<PlaySceneHandlerController>();
        }

        private void InstallHandlers()
        {
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneFirstHandler>(nameof(ScenePlayerConst.Robot));
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneSecondHandler>(nameof(ScenePlayerConst.Woman));
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneFirstHandler>(nameof(ScenePlayerConst.it0));
        }

        private void InstallRules()
        {
            Container.BindService<StopSceneRule>();
            Container.BindService<PlaySceneRule>();
            Container.BindService<UpdateDirectionLightForwardRule>();
        }

        private void InstallSignals()
        {
            Container.DeclareSignal<ScenePlayerSignals.SceneFinished>();
        }

        private void InstallFactories()
        {
            Container
                .BindInterfacesAndSelfTo<PlaySceneHandlerFactory>()
                .AsSingle();
        }
    }
}