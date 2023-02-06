using Features.ScenePlayer.Controllers;

namespace Features.ScenePlayer.Handlers
{
    public class BasePlaySceneHandler : IPlaySceneHandler
    {
        protected readonly ScenePlayerComponentsController ScenePlayerComponentsController;
        protected string SceneName { get; private set; }

        public BasePlaySceneHandler(ScenePlayerComponentsController scenePlayerComponentsController)
        {
            ScenePlayerComponentsController = scenePlayerComponentsController;
        }

        public virtual void Initialize(string sceneName)
        {
            SceneName = sceneName;
            ScenePlayerComponentsController.PlayScene(SceneName);
        }

        public virtual void Dispose()
        {
            ScenePlayerComponentsController.StopScene();
        }
    }
}
