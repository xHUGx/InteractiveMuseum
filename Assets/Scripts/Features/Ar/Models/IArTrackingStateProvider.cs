using System;
using Features.Ar.Data;

namespace Features.Ar.Models
{
    public interface IArTrackingStateProvider
    {
        bool IsActive { get; }
        string ImageName { get; }
        bool GetIsTracked();
        IObservable<bool> GetIsTrackedAsObservable();
        PositionData GetPosition();
        IObservable<PositionData> GetPositionAsObservable();
    }
}