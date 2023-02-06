using System;

namespace Features.DebugSystem.Models
{
    public interface IDebugGraphicsProvider
    {
        bool GetIsEnabled();
        IObservable<bool> GetIsEnabledAsObservable();
    }
}