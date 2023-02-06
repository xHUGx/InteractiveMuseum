using System;
using Features.LightSystem.Factories;
using Features.LightSystem.Views;
using UnityEngine;
using Zenject;

namespace Features.LightSystem.Services
{
    public class DirectionalLightService : IInitializable, IDisposable
    {
        private readonly DirectionalLightViewFactory _directionalLightViewFactory;

        private DirectionalLightView _directionalLightView;

        public DirectionalLightService(DirectionalLightViewFactory directionalLightViewFactory)
        {
            _directionalLightViewFactory = directionalLightViewFactory;
        }

        public void Initialize()
        {
            _directionalLightView = _directionalLightViewFactory.Create();
        }

        public void UpdateForwardDirection(Vector3 direction)
        {
            _directionalLightView.UpdateForwardDirection(direction);
        }

        public void Dispose()
        {
            if (_directionalLightView != null)
                _directionalLightView.Dispose();
        }
    }
}