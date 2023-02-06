using EffectSystem;
using Features.ScenePlayer.Controllers;

namespace Features.ScenePlayer.Handlers
{
    public class PlaySceneRootAndBloomHandler : BasePlaySceneHandler
    {
        private readonly EffectsController _effectsController;

        public PlaySceneRootAndBloomHandler(ScenePlayerComponentsController scenePlayerComponentsController, EffectsController effectsController)
            : base(scenePlayerComponentsController)
        {
            _effectsController = effectsController;
        }

        public override void Dispose()
        {
            base.Dispose();

            _effectsController.ResetActiveEffects();
        }
    }
}
