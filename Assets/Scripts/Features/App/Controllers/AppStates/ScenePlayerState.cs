using Cysharp.Threading.Tasks;
using Features.App.Data;
using Features.ScenePlayer.Models;

namespace Features.App.Controllers.AppStates
{
    public class ScenePlayerState : AppStateBase
    {
        public override AppStateType StateType => AppStateType.ScenePlayer;

        private readonly ScenePlayerModel _scenePlayerModel;

        private readonly ScreenSleepController _screenSleepController;

        public ScenePlayerState(ScenePlayerModel scenePlayerModel,
            ScreenSleepController screenSleepController)
        {
            _screenSleepController = screenSleepController;
            _scenePlayerModel = scenePlayerModel;
        }

        public override async UniTask Initialize()
        {
            await base.Initialize();
            
            _screenSleepController.SetAlwaysOn();

            await UniTask.SwitchToMainThread();

            _scenePlayerModel.Enable();

            await UniTask.Delay(100);

            State.Value = StateEventType.Stay;
        }

        public override async UniTask Dispose()
        {
            _screenSleepController.SetDefault();

            _scenePlayerModel.Disable();

            await UniTask.Delay(100);

            State.Value = StateEventType.Exited;
        }
    }
}