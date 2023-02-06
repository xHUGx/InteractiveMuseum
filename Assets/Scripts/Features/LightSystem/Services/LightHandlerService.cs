using System;
using System.Collections.Generic;
using Features.LightSystem.Messages;
using Features.LightSystem.Views;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.LightSystem.Services
{
    public class LightHandlerService : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;

        private readonly CompositeDisposable _compositeDisposable;

        private readonly Dictionary<string, DirectionalLightHandlerView> _directionLightHandlers;

        public LightHandlerService(SignalBus signalBus)
        {
            _signalBus = signalBus;

            _compositeDisposable = new CompositeDisposable();
            _directionLightHandlers = new Dictionary<string, DirectionalLightHandlerView>();
        }

        public void Initialize()
        {
            _signalBus
                .GetStream<LightSignals.RegisterNewDirectionalLightHandler>()
                .Subscribe(signal =>
                {
                    if (string.IsNullOrEmpty(signal.Id) || signal.DirectionalLightHandlerView == null) return;

                    _directionLightHandlers.TryAdd(signal.Id, signal.DirectionalLightHandlerView);
                })
                .AddTo(_compositeDisposable);
        }

        public bool TryGetLightForwardDirection(string id, out Vector3 forward)
        {
            forward = Vector3.zero;
            if (!_directionLightHandlers.TryGetValue(id, out var handler)) return false;
            forward = handler.GetForwardDirection();
            return true;
        }
        
        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            _directionLightHandlers?.Clear();
        }
    }
}