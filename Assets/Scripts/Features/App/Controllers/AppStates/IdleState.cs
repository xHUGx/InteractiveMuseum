using Cysharp.Threading.Tasks;
using Features.App.Controllers;
using Features.App.Data;

namespace Features.App.Controllers.AppStates
{
    public class IdleState : AppStateBase
    {
        public override AppStateType StateType => AppStateType.Idle;

        public override async UniTask Initialize()
        {
            await base.Initialize();
            State.Value = StateEventType.Stay;
        }
    }
}