using System;
using UniRx;

namespace Features.DebugSystem.Models
{
    public class DebugArImageInfoEnabledModel : IDebugArImageInfoEnabledProvider
    {
        private readonly ReactiveProperty<bool> _isEnabledImagePresenter = new ReactiveProperty<bool>(false);

        public bool GetIsEnabledImageInfo() => _isEnabledImagePresenter.Value;
        public IObservable<bool> GetIsEnabledImageInfoAsObservable() => _isEnabledImagePresenter.AsObservable();
        
        public void UpdateImageInfoIsEnabled(bool value)
        {
            _isEnabledImagePresenter.Value = value;
        }
    }
}