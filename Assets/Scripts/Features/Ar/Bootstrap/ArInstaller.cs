using Features.Ar.Factories;
using Features.Ar.Messages;
using Features.Ar.Models;
using Features.Ar.Rules;
using Features.Ar.Services;
using UnityEngine;
using Utils;
using Zenject;

namespace Features.Ar.Bootstrap
{
    public class ArInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("[ArInstaller] Installing");

            // Container = ProjectContext.Instance.Container;
            
            InstallModels();
            InstallRules();
            InstallSignals();
            InstallFactories();
            InstallServices();
        }

        private void InstallModels()
        {
            Container.BindService<ArComponentsModel>();
            Container.BindService<ArTrackingModel>();
        }

        private void InstallRules()
        {
            // Container.BindRule<ImageTrackingImageFoundRule>();
            // Container.BindRule<ImageTrackingImageLostRule>();
            // Container.BindRule<ImageTrackingResetTrackingRule>();
            Container.BindRule<ImageTrackingUIRule>();
            Container.BindRule<SceneInCameraFrustumRule>();
        }

        private void InstallSignals()
        {
            // Container.DeclareSignal<ArSignals.ImageFound>();
            // Container.DeclareSignal<ArSignals.ImageLost>();
            Container.DeclareSignal<ArSignals.ResetTracking>();
            Container.DeclareSignal<ArSignals.RegisterNewImageAnchor>();
        }

        private void InstallFactories()
        {
            Container
                .Bind<ArComponentsViewFactory>()
                .AsSingle();
            
            Container
                .Bind<PositionAverageFilterFactory>()
                .AsSingle();

            Container
                .Bind<DirectionAverageFilterFactory>()
                .AsSingle();
        }

        private void InstallServices()
        {
            Container.BindService<ArComponentsService>();
            Container.BindService<ArImageAnchorService>();
            Container.BindService<ArImageTrackingService>();
            Container.BindService<PointsInCameraFrustumService>();
        }
    }
}