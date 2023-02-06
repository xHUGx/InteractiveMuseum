using Features.ScenePlayer.Models;
using Features.ScenePlayer.Views;
using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Features.ScenePlayer.Controllers
{
    public class ScenePlayerComponentsController : IDisposable
    {
        private const int DelayBeforeInitScenePlayers = 200;
        private readonly ScenePlayerModel _scenePlayerModel;

        private ScenePlayerComponentsView _scenePlayerComponentsView;

        public bool IsReady
        {
            get => _scenePlayerComponentsView != null;
        }

        private IDisposable _disposable;

        public async void Bind(ScenePlayerComponentsView scenePlayerComponentsView)
        {
            _scenePlayerComponentsView = scenePlayerComponentsView;
            
            // Awaiting while all Awakes are executed 
            await UniTask.Delay(DelayBeforeInitScenePlayers);
            _scenePlayerComponentsView.Initialize();
        }

        public void PlayScene(string sceneName)
        {
            if (!_scenePlayerComponentsView.TryGetScenePlayer(sceneName, out var scenePlayer)) return;
            scenePlayer.PlayScene();
        }

        public void StopScene()
        {
            _scenePlayerComponentsView.HideScenes();
        }

        public void Dispose()
        {
            if (_scenePlayerComponentsView != null)
                _scenePlayerComponentsView.HideScenes();
        }
    }
}