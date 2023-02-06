using System;
using Cysharp.Threading.Tasks;
using Features.App.Controllers;
using Features.App.Data;

namespace Features.App.Controllers.AppStates
{
    public interface IAppState
    {
        UniTask Initialize();
        UniTask Dispose();
        AppStateType StateType { get; }

        IObservable<StateEventType> GetStateEventTypeAsObservable();
    }
}