using System;
using Features.Ar.Messages;
using Features.Ar.Models;
using Features.Ar.Services;
using Zenject;
using UniRx;

namespace Features.Ar.Rules
{
    public class SceneInCameraFrustumRule : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly ArTrackingModel _arTrackingModel;
        private readonly PointsInCameraFrustumService _pointsInCameraFrustumService;

        private readonly CompositeDisposable _compositeDisposable;

        private IDisposable _pointInCameraFrustumStream;

        public SceneInCameraFrustumRule(SignalBus signalBus,
            ArTrackingModel arTrackingModel,
            PointsInCameraFrustumService pointsInCameraFrustumService)
        {
            _signalBus = signalBus;
            _arTrackingModel = arTrackingModel;
            _pointsInCameraFrustumService = pointsInCameraFrustumService;

            _compositeDisposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            _arTrackingModel
                .GetIsTrackedAsObservable()
                .Where(value => value)
                .Subscribe(_ =>
                {
                    _pointsInCameraFrustumService.StopFollow();
                    _pointInCameraFrustumStream?.Dispose();
                    
                    if (_pointsInCameraFrustumService.TryStartFollow(_arTrackingModel.GetContentBoundsPositions()))
                    {
                        _pointInCameraFrustumStream = _pointsInCameraFrustumService
                            .GetIsOutOfFrustumAsObservable()
                            .Subscribe(_ =>
                            {
                                _signalBus.TryFire(new ArSignals.ResetTracking());
                                _pointInCameraFrustumStream?.Dispose();
                            });
                        
                        
                        
                    }
                })
                .AddTo(_compositeDisposable);


            _arTrackingModel
                .GetIsTrackedAsObservable()
                .Where(value => !value)
                .Subscribe(_ =>
                {
                    _pointsInCameraFrustumService.StopFollow();
                    _pointInCameraFrustumStream?.Dispose();
                })
                .AddTo(_compositeDisposable);

        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}