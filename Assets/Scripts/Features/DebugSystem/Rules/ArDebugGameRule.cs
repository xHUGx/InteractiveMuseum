using System;
using System.Text;
using Features.Ar.Data;
using Features.Ar.Messages;
using Features.Ar.Models;
using Features.Ar.Services;
using Features.DebugSystem.Config;
using Features.DebugSystem.Models;
using Features.DebugSystem.Services;
using SRDebugger;
using SRDebugger.Services;
using UniRx;
using Zenject;

namespace Features.DebugSystem.Rule
{
    public class ArDebugGameRule : IInitializable, IDisposable
    {
        private readonly string ArContainerCategory = "Ar tracking";
        private readonly string ArShowTrackedImageOptionName = "Show tracked image";
        private readonly string ArResetTrackingOptionName = "Reset tracking";
        private readonly string ArTrackedPositionsCountOptionName = "Tracked Positions Count";
        private readonly SignalBus _signalBus;
        private readonly ArImageTrackingService _arImageTrackingService;
        private readonly DebugSettings _debugSettings;
        private readonly IDebugService _debugService;

        private readonly DebugArImageTrackingService _debugArImageTrackingService;
        private readonly DebugArImageInfoEnabledModel _debugArImageInfoEnabledModel;
        private readonly CompositeDisposable _compositeDisposable;

        public ArDebugGameRule(ArTrackingModel arTrackingModel,
            SignalBus signalBus,
            ArImageTrackingService arImageTrackingService,
            [InjectOptional] DebugArImageTrackingService debugArImageTrackingService,
            [InjectOptional] DebugArImageInfoEnabledModel debugArImageInfoEnabledModel,
            [InjectOptional] DebugSettings debugSettings, 
            [InjectOptional] IDebugService debugService)
        {
            _signalBus = signalBus;
            _arImageTrackingService = arImageTrackingService;
            _debugArImageInfoEnabledModel = debugArImageInfoEnabledModel;
            _debugArImageTrackingService = debugArImageTrackingService;
            _debugSettings = debugSettings;
            _debugService = debugService;
            _compositeDisposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            if (_debugSettings == null)
                return;

            _debugArImageInfoEnabledModel.UpdateImageInfoIsEnabled(_debugSettings.IsShowTrackedImages);

            var container = new DynamicOptionContainer();
            var imagePreviewOption =
                OptionDefinition.Create(ArShowTrackedImageOptionName,
                    () => _debugArImageInfoEnabledModel.GetIsEnabledImageInfo(),
                    value => { _debugArImageInfoEnabledModel.UpdateImageInfoIsEnabled(value); }, ArContainerCategory);

            var resetTracking = OptionDefinition.FromMethod(ArResetTrackingOptionName,
                () => { _signalBus.TryFire<ArSignals.ResetTracking>(); }, ArContainerCategory);

            var imagesIds = ArImageTypesConst.GetAllImageTypes();
            var stringBuilder = new StringBuilder();

            var trackedPositionsOption =
                OptionDefinition.Create(ArTrackedPositionsCountOptionName,
                    () =>
                    {
                        stringBuilder.Clear();
                        foreach (var imagesId in imagesIds)
                        {
                            stringBuilder.AppendLine(imagesId + ": " +
                                                     _arImageTrackingService.GetPositionsCountByImageId(imagesId));
                        }

                        return stringBuilder.ToString();
                    },
                    category: ArContainerCategory);

            container.AddOption(imagePreviewOption);
            container.AddOption(resetTracking);
            container.AddOption(trackedPositionsOption);

            _debugService.AddOptionContainer(container);

            _debugService.PinOption(ArResetTrackingOptionName);
            // _debugService.PinOption(ArTrackedPositionsCountOptionName);

            Observable
                .Interval(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => { trackedPositionsOption.Property?.NotifyValueChanged(); })
                .AddTo(_compositeDisposable);
            
            
            _debugArImageTrackingService.Enable();
        }


        public void Dispose()
        {
            _debugArImageTrackingService.Disable();
            _compositeDisposable?.Dispose();
        }
    }
}