using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.App.Data;
using Features.App.Factories;
using Features.SceneManagement.Config;
using Features.SceneManagement.Services;
using UniRx;
using UnityEngine;
using Zenject;

// ReSharper disable once CheckNamespace
namespace Features.App.Controllers
{
    public class AppController : IInitializable, IDisposable
    {
        private readonly AppModel _appModel;

        private readonly AppStatesFactory _appStatesFactory;
        private readonly CompositeDisposable _compositeDisposable;
        
        private readonly Queue<AppStateType> _appStatesQueue;

        private IDisposable _executeStream;
        

        public AppController(AppModel appModel,
            AppStatesFactory appStatesFactory
        )
        {
            _appModel = appModel;
            _appStatesFactory = appStatesFactory;

            // TODO: move value to config and allocate to own rule
            Application.targetFrameRate = 60;

            _appStatesQueue = new Queue<AppStateType>();
            _compositeDisposable = new CompositeDisposable();
        }

        public async void Initialize()
        {
            await UniTask.Delay(50);
            SetAppState(AppStateType.Loading);

            /*
            Observable
                .EveryApplicationPause()
                .Subscribe(_ => 
                {
                    SetAppState(AppStateType.ImageTracking);
                })
                .AddTo(_compositeDisposable);
            */
        }

        public void SetAppState(AppStateType appState)
        {
            // Debug.Log($"[AppController] Add app state to queue: {appState}");
            _appStatesQueue.Enqueue(appState);
            ExecuteQueue();
        }


        private void ExecuteQueue()
        {
            if (_executeStream != null) return;

            _executeStream = Observable
                .NextFrame()
                .Subscribe(async _ =>
                {
                    while (_appStatesQueue.Count > 0)
                    {
                        // Debug.Log($"[AppController] Queue count: {_appStatesQueue.Count}");
                        var state = _appStatesQueue.Dequeue();
                        // Debug.Log($"[AppController] Dequeue state: {state}");
                        // Debug.Log($"[AppController] Queue count: {_appStatesQueue.Count}");
                        await ApplyAppState(state);
                    }

                    _executeStream?.Dispose();
                    _executeStream = null;
                });
        }

        private async UniTask ApplyAppState(AppStateType appState)
        {
            if (_appModel.GetAppState().AppState == appState) return;

            await _appModel.GetAppStateController().Dispose();

            var newState = _appStatesFactory.Create(appState);
            _appModel.SetAppStateController(newState);

            await newState.Initialize();
        }

        public void Dispose()
        {
            _executeStream?.Dispose();
            _compositeDisposable?.Dispose();
        }
    }
}