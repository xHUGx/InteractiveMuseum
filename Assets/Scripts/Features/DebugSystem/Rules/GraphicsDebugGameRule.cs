using System;
using Features.Ar.Messages;
using Features.Ar.Models;
using Features.DebugSystem.Config;
using Features.DebugSystem.Models;
using SRDebugger;
using SRDebugger.Services;
using Zenject;

namespace Features.DebugSystem.Rule
{
    public class GraphicsDebugGameRule : IInitializable, IDisposable
    {
        private readonly string ContainerCategory = "Graphics";
        private readonly string ShowDebugGraphicsOptionName = "Show debug scene objects";


        private readonly SignalBus _signalBus;
        private readonly DebugSettings _debugSettings;
        private readonly IDebugService _debugService;

        private readonly DebugGraphicsModel _debugGraphicsModel;

        public GraphicsDebugGameRule(SignalBus signalBus,
            [InjectOptional] DebugGraphicsModel debugGraphicsModel,
            [InjectOptional] DebugSettings debugSettings,
            [InjectOptional] IDebugService debugService)
        {
            _signalBus = signalBus;
            _debugSettings = debugSettings;
            _debugService = debugService;
            _debugGraphicsModel = debugGraphicsModel;
        }

        public void Initialize()
        {
            if (_debugSettings == null)
                return;

            _debugGraphicsModel.UpdateIsEnabledDebugGraphics(_debugSettings.IsShowDebugGraphics);

            var container = new DynamicOptionContainer();
            var graphicsOption =
                OptionDefinition.Create(ShowDebugGraphicsOptionName,
                    () => _debugGraphicsModel.GetIsEnabled(),
                    value => { _debugGraphicsModel.UpdateIsEnabledDebugGraphics(value); }, ContainerCategory);

            container.AddOption(graphicsOption);

            _debugService.AddOptionContainer(container);
            
            _debugService.PinOption(ShowDebugGraphicsOptionName);
        }


        public void Dispose()
        {
        }
    }
}