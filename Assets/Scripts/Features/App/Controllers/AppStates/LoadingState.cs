using Cysharp.Threading.Tasks;
using Features.App.Data;
using Features.SceneManagement.Services;
using SceneType = Features.SceneManagement.Data.SceneType;
using Zenject;
using Features.DebugSystem.Config;

namespace Features.App.Controllers.AppStates
{
    public class LoadingState : AppStateBase
    {
        public override AppStateType StateType => AppStateType.Loading;

        private readonly ISceneLoaderService _sceneLoaderService;

        public LoadingState(ISceneLoaderService sceneLoaderService)
        {
            _sceneLoaderService = sceneLoaderService;
        }

        public override async UniTask Initialize()
        {
            await base.Initialize();

            await _sceneLoaderService.LoadScene(SceneType.ScenePlayer);
            // await _sceneLoaderService.LoadScene(SceneType.ImageTracking);

            await UniTask.Delay(1000);
            State.Value = StateEventType.Stay;
        }
    }
}