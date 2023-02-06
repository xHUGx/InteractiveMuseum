using System;
using Features.Ar.Messages;
using Features.Ar.Models;
using Features.DebugSystem.Config;
using Features.DebugSystem.Data;
using SRDebugger;
using SRDebugger.Services;
using UniRx;
using UnityEngine;
using UnityTemplateProjects;
using Zenject;

namespace Features.DebugSystem.Rule
{
    public class EditorDebugRule : IInitializable, IDisposable
    {
        private readonly string ArContainerCategory = "Ar tracking";

        private readonly SignalBus _signalBus;
        private readonly DebugSettings _debugSettings;
        private readonly IDebugService _debugService;
        private readonly ArComponentsModel _arComponentsModel;
        private readonly DiContainer _container;

        private GameObject _background;
        private IDisposable _arReadyStream;

        public EditorDebugRule(ArComponentsModel arComponentsModel,
            SignalBus signalBus,
            DiContainer container,
            [InjectOptional] DebugSettings debugSettings)
        {
            _signalBus = signalBus;
            _arComponentsModel = arComponentsModel;
            _container = container;
            _debugSettings = debugSettings;
        }

        public void Initialize()
        {
            if (_debugSettings == null)
                return;

            if (!Application.isEditor) return;

            if (_debugSettings.IsShowBackground)
            {
                _background = _container
                    .InstantiatePrefabResource(DebugResources.DebugBackground);
            }

            if (_debugSettings.IsAddSimpleCameraController)
            {
                _arReadyStream = _arComponentsModel
                    .GetIsReadyAsObservable()
                    .Where(value => value)
                    .Subscribe(_ =>
                    {
                        _arComponentsModel.CameraView.gameObject.AddComponent<SimpleCameraController>();
                    });
            }
            
            if (_debugSettings.IsShowContentBoundPositions)
            {
                _background = _container
                    .InstantiatePrefabResource(DebugResources.DebugContentBoundsPositions);
            }
        }


        public void Dispose()
        {
            _arReadyStream?.Dispose();

            if (_background != null)
            {
                UnityEngine.Object.Destroy(_background);
            }
        }
    }
}