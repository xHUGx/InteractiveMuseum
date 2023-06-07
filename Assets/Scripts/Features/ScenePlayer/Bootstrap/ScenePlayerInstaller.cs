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
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneRobotHandler>(nameof(ScenePlayerConst.Robot));
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneWomanHandler>(nameof(ScenePlayerConst.Woman));
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneDogHandler>(nameof(ScenePlayerConst.Dog));
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneTabletsHandler>(nameof(ScenePlayerConst.it0));
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneTabletsHandler>(nameof(ScenePlayerConst.it1));
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneTabletsHandler>(nameof(ScenePlayerConst.it2));
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneTabletsHandler>(nameof(ScenePlayerConst.it3));
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneTabletsHandler>(nameof(ScenePlayerConst.it4));
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneTabletsHandler>(nameof(ScenePlayerConst.it5));
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneTabletsHandler>(nameof(ScenePlayerConst.it6));
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneTabletsHandler>(nameof(ScenePlayerConst.it7));
            Container.InstallAsTransient<IPlaySceneHandler, PlaySceneTabletsHandler>(nameof(ScenePlayerConst.it8));
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