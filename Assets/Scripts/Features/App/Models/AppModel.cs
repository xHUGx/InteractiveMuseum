using System;
using Features.App.Data;
using Features.App.Controllers.AppStates;
using UniRx;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Features.App.Controllers
{
    public enum StateEventType
    {
        Idle,
        Enter,
        Stay,
        Exited
    }

    [Serializable]
    public class AppStateData
    {
        public AppStateType AppState;
        public StateEventType EventType;

        public override string ToString()
        {
            return $"App state: <b>{AppState}</b>, Event type: <b>{EventType}</b>";
        }
    }

    public interface IAppModel
    {
        IObservable<AppStateData> GetAppStateAsObservable();
        AppStateData GetAppState();
    }

    public class AppModel : IAppModel
    {
        private ReactiveProperty<AppStateData> _appStateReactive;
        private IAppState _appStateController;

        private IDisposable _eventsStream;

        public AppModel()
        {
            var startState = new IdleState();
            SetAppStateController(startState);
            startState.Initialize();
        }

        public IObservable<AppStateData> GetAppStateAsObservable()
        {
            return _appStateReactive.AsObservable();
        }

        public AppStateData GetAppState()
        {
            return _appStateReactive.Value;
        }


        public IAppState GetAppStateController()
        {
            return _appStateController;
        }

        public void SetAppStateController(IAppState appStateController)
        {
            if (appStateController == null) return;

            _eventsStream?.Dispose();

            _appStateController = appStateController;

            _eventsStream = _appStateController
                .GetStateEventTypeAsObservable()
                .Subscribe(value =>
                {
                    var newState = new AppStateData()
                    {
                        AppState = _appStateController.StateType,
                        EventType = value
                    };

                    if (_appStateReactive == null)
                    {
                        _appStateReactive = new ReactiveProperty<AppStateData>(newState);
                    }
                    else
                    {
                        _appStateReactive.Value = newState;
                    }

                    Debug.Log($"[AppModel] Changed state to {newState}");
                });
        }
    }
}