using System;
using Features.Ar.Data;
using Features.Ar.Messages;
using Features.Ar.Models;
using Zenject;
using UnityEngine;
using Rule;

// ReSharper disable once CheckNamespace
namespace Features.Ar.Rules
{
    // public class ImageTrackingImageFoundRule : AbstractSignalRule<ArSignals.ImageFound>
    // {
    //     private readonly ArTrackingModel _arTrackingModel;
    //
    //     public ImageTrackingImageFoundRule(ArTrackingModel arTrackingModel)
    //     {
    //         _arTrackingModel = arTrackingModel;
    //     }
    //
    //     protected override void OnSignalFired(ArSignals.ImageFound signal)
    //     {
    //         // var positionData = new PositionData()
    //         // {
    //         //     Position = signal.Anchor.position,
    //         //     Rotation = signal.Anchor.rotation
    //         // };
    //         //
    //         // _arTrackingModel.UpdateIsTracked(true, positionData, signal.Name);
    //     }
    // }
}