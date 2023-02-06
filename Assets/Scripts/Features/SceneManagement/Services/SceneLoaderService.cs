using Cysharp.Threading.Tasks;
using Features.SceneManagement.Config;
using Features.SceneManagement.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

// ReSharper disable once CheckNamespace
namespace Features.SceneManagement.Services
{
    public interface ISceneLoaderService
    {
        UniTask<bool> LoadScene(SceneType sceneType);
        UniTask UnloadScene(SceneType sceneType);
    }

    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly ZenjectSceneLoader _sceneLoader;
        private readonly SceneRegistry _sceneRegistry;

        public SceneLoaderService(ZenjectSceneLoader sceneLoader, SceneRegistry sceneRegistry)
        {
            _sceneLoader = sceneLoader;
            _sceneRegistry = sceneRegistry;
        }

        public async UniTask<bool> LoadScene(SceneType sceneType)
        {
            var result = false;

            var sceneName = sceneType.ToString();

            if (_sceneRegistry.TryGetById(sceneName, out var sceneRegistryItem))
            {
                Debug.Log($"[SceneLoaderService] Loading scene: {sceneName}");
                await _sceneLoader.LoadSceneAsync(sceneName, sceneRegistryItem.LoadMode).ToUniTask();
                result = true;
            }

            return result;
        }

        public async UniTask UnloadScene(SceneType sceneType)
        {
            var sceneName = sceneType.ToString();

            Debug.Log($"[SceneLoaderService] Unloading scene: {sceneName}");

            if (_sceneRegistry.TryGetById(sceneName, out var sceneRegistryItem))
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    if (SceneManager.GetSceneAt(i).name == sceneName)
                    {
                        await SceneManager.UnloadSceneAsync(sceneName);
                    }
                }
            }
        }
    }
}