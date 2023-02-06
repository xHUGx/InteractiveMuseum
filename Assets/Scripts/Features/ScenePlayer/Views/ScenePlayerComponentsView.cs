using Features.DebugSystem.Config;
using Features.ScenePlayer.Controllers;
using UnityEngine;
using Zenject;

namespace Features.ScenePlayer.Views
{
    public class ScenePlayerComponentsView : MonoBehaviour
    {
        [SerializeField] private ScenePlayerView[] _scenes;
        [SerializeField] private GameObject _testCamera;


        [Inject]
        private void Construct(ScenePlayerComponentsController scenePlayerComponentsController)
        {
            scenePlayerComponentsController.Bind(this);
        }

        public void Initialize()
        {
            foreach (var scene in _scenes)
            {
                if (scene != null)
                    scene.Initialize();
            }
        }

        public bool TryGetScenePlayer(string sceneName, out ScenePlayerView scenePlayerView)
        {
            foreach (var scene in _scenes)
            {
                if (scene != null && scene.SceneName.Equals(sceneName))
                {
                    scenePlayerView = scene;
                    return true;
                }
            }

            scenePlayerView = null;
            return false;
        }

        public void HideScenes()
        {
            foreach (var scene in _scenes)
            {
                if (scene != null)
                    scene.StopScene();
            }
        }
    }
}