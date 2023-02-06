using System;
using Zenject;
using UniRx;
using Features.LightSystem.Services;
using Features.ScenePlayer.Models;

namespace Features.ScenePlayer.Rules
{
    public class UpdateDirectionLightForwardRule : IInitializable, IDisposable
    {
        private readonly ScenePlayerModel _scenePlayerModel;
        private readonly DirectionalLightService _directionalLightService;
        private readonly LightHandlerService _lightHandlerService;
        private readonly CompositeDisposable _compositeDisposable;

        public UpdateDirectionLightForwardRule(ScenePlayerModel scenePlayerModel,
            DirectionalLightService directionalLightService,
            LightHandlerService lightHandlerService)
        {
            _scenePlayerModel = scenePlayerModel;
            _directionalLightService = directionalLightService;
            _lightHandlerService = lightHandlerService;
            _compositeDisposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            _scenePlayerModel
                .GetIsPlayingAsObservable()
                .Where(value => value)
                .Subscribe(value =>
                {
                    if (!_lightHandlerService.TryGetLightForwardDirection(_scenePlayerModel.SceneName, out var forward))
                        return;
                    _directionalLightService.UpdateForwardDirection(forward);
                })
                .AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}