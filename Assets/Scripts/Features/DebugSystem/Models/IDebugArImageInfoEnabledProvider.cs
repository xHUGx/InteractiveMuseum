using System;

namespace Features.DebugSystem.Models
{
    public interface IDebugArImageInfoEnabledProvider 
    {
        bool GetIsEnabledImageInfo();
        IObservable<bool> GetIsEnabledImageInfoAsObservable();
    }
}