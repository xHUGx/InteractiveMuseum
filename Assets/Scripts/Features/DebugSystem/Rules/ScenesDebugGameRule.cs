using System;
using Features.Ar.Data;
using Features.Ar.Messages;
using Features.Ar.Models;
using Features.Ar.Services;
using Features.DebugSystem.Config;
using Features.DebugSystem.Data;
using Features.UI.View;
using SRDebugger;
using SRDebugger.Services;
using Zenject;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.DebugSystem.Rule
{
    public class ScenesDebugGameRule : IInitializable, IDisposable
    {
        private readonly int ScenePositionUpdateIntervalMs = 250;
        private readonly string ScenesCategory = "Scenes";

        private readonly SignalBus _signalBus;
        private readonly DebugSettings _debugSettings;
        private readonly IDebugService _debugService;
        private readonly ArComponentsModel _arComponentsModel;
        private readonly ArTrackingModel _arTrackingModel;
        private readonly ArImageAnchorService _arImageAnchorService;
        private readonly DiContainer _diContainer;

        private DebugArImageAnchorsView _debugArImageAnchorsView;

        private IDisposable _resetStream;
        private IDisposable _arReadyStream;

        private IDisposable _updateScenePositionStream;

        private float _distanceToScene = 2f;
        private bool _lockUpdateTrackingPosition;


        public ScenesDebugGameRule(SignalBus signalBus,
            ArComponentsModel arComponentsModel,
            ArTrackingModel arTrackingModel,
            DiContainer diContainer,
            ArImageAnchorService arImageAnchorService,
            [InjectOptional] DebugSettings debugSettings,
            [InjectOptional] IDebugService debugService)
        {
            _signalBus = signalBus;
            _diContainer = diContainer;
            _arComponentsModel = arComponentsModel;
            _arTrackingModel = arTrackingModel;
            _arImageAnchorService = arImageAnchorService;
            _debugSettings = debugSettings;
            _debugService = debugService;
        }

        public void Initialize()
        {
            if (_debugSettings == null)
                return;

            var container = new DynamicOptionContainer();

            if (_debugSettings.IsShowButtonsToMockTracking)
            {
                _debugArImageAnchorsView = _diContainer
                    .InstantiatePrefabResourceForComponent<DebugArImageAnchorsView>(DebugResources.DebugArImageAnchors);

                var imageNames = ArImageTypesConst.GetAllImageTypes();

                foreach (var imageName in imageNames)
                {
                    var option = OptionDefinition.FromMethod(imageName, () =>
                    {
                        _updateScenePositionStream?.Dispose();

                        if (_arTrackingModel.GetIsTracked()) return;

                        if (!_debugArImageAnchorsView.TryGetArAnchorPosition(imageName, out var positionData)) return;

                        var cameraRotation = Quaternion.Euler(0f,
                            _arComponentsModel.CameraView.Transform.rotation.eulerAngles.y,
                            0f);
                        var cameraPosition = _arComponentsModel.CameraView.Transform.position;
                        
                        positionData.Rotation = cameraRotation * positionData.Rotation;
                        positionData.Position = cameraPosition + cameraRotation * positionData.Position +
                                                cameraRotation * (Vector3.forward * _distanceToScene);


                        if (_arImageAnchorService.TryGetImageAnchorContentBoundPositions(imageName, out var positions))
                        {
                            _arTrackingModel.UpdateContentBoundPositions(positions);
                        }

                        _arTrackingModel.UpdateIsTracked(true, positionData, imageName);


                        if (_debugSettings.IsSimulateTrackedImageMoving)
                        {
                            _updateScenePositionStream = Observable
                                .Interval(TimeSpan.FromMilliseconds(ScenePositionUpdateIntervalMs))
                                .Subscribe(_ =>
                                {
                                    if (!_arTrackingModel.GetIsTracked() || !_debugSettings.IsSimulateTrackedImageMoving) return;

                                    var newPosition = _arTrackingModel.GetPosition();

                                    newPosition.Position = new Vector3(
                                        Random.Range(newPosition.Position.x - 0.3f,
                                            newPosition.Position.x + 0.3f),
                                        Random.Range(newPosition.Position.y - 0.3f,
                                            newPosition.Position.y + 0.3f),
                                        Random.Range(newPosition.Position.z - 0.3f,
                                            newPosition.Position.z + 0.3f));
                                    
                                    _arTrackingModel.UpdateIsTracked(true, newPosition, imageName);
                                });
                        }
                    }, ScenesCategory);

                    container.AddOption(option);
                }

                _debugService.AddOptionContainer(container);

                _debugService.PinAllOptions(ScenesCategory);
            }
        }


        public void Dispose()
        {
            _resetStream?.Dispose();
            if (_debugArImageAnchorsView != null)
                _debugArImageAnchorsView.Dispose();
        }
    }
}