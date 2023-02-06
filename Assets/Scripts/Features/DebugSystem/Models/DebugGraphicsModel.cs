using System;
using UniRx;

namespace Features.DebugSystem.Models
{
    public class DebugGraphicsModel : IDebugGraphicsProvider
    {
        private readonly ReactiveProperty<bool> _isEnabledDebugGraphics = new ReactiveProperty<bool>(false);
        
        public bool GetIsEnabled() =>
            _isEnabledDebugGraphics.Value;

        public IObservable<bool> GetIsEnabledAsObservable()
            => _isEnabledDebugGraphics.AsObservable();


        public void UpdateIsEnabledDebugGraphics(bool value)
        {
            _isEnabledDebugGraphics.Value = value;
        }
    }
}