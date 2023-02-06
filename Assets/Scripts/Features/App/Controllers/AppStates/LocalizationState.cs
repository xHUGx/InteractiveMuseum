using Cysharp.Threading.Tasks;
using Features.App.Data;
using Features.Ar.Models;
using Features.Ar.Services;

namespace Features.App.Controllers.AppStates
{
    public class LocalizationState : AppStateBase
    {
        public override AppStateType StateType => AppStateType.Localization;

        private readonly ArComponentsService _arComponentsService;
        private readonly ArTrackingModel _arTrackingModel;

        private readonly ArImageTrackingService _arImageTrackingService;
        private readonly ScreenSleepController _screenSleepController;

        public LocalizationState(ArComponentsService arComponentsService,
            ArTrackingModel arTrackingModel,
            ScreenSleepController screenSleepController,
            ArImageTrackingService arImageTrackingService)
        {
            _arComponentsService = arComponentsService;
            _screenSleepController = screenSleepController;
            _arTrackingModel = arTrackingModel;
            _arImageTrackingService = arImageTrackingService;
        }

        public override async UniTask Initialize()
        {
            await base.Initialize();
            
            _screenSleepController.SetAlwaysOn();
            _arComponentsService.Enable();

            await UniTask.SwitchToMainThread();

            _arImageTrackingService.Enable();
            // _imageTrackingModel.Enable();

            await UniTask.Delay(100);

            State.Value = StateEventType.Stay;
        }

        public override async UniTask Dispose()
        {
            _screenSleepController.SetDefault();

            //_arImageTrackingService.Disable();
            await UniTask.Delay(100);

            State.Value = StateEventType.Exited;
        }
    }
}