using System;
using Cysharp.Threading.Tasks;
using Features.App.Data;
using UniRx;

namespace Features.App.Controllers.AppStates
{
    public abstract class AppStateBase : IAppState
    {
        protected readonly ReactiveProperty<StateEventType> State = new (StateEventType.Idle);
     
        public abstract AppStateType StateType { get; }

        public virtual async UniTask Initialize()
        {
            State.Value = StateEventType.Enter;
            await UniTask.Yield();
        }

        public virtual async UniTask Dispose()
        {
            await UniTask.NextFrame();
            State.Value = StateEventType.Exited;
        }

        public IObservable<StateEventType> GetStateEventTypeAsObservable()
        {
            return State.AsObservable();
        }
    }
}