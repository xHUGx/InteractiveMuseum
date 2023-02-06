using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.Ar.Configs;
using Features.Ar.Controllers;
using Features.Ar.Data;
using Features.Ar.Factories;
using Features.Ar.Messages;
using Features.Ar.Models;
using Features.Ar.Views;
using UnityEngine.XR.ARFoundation;
using Zenject;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

namespace Features.Ar.Services
{
    public class ArImageTrackingService : IInitializable, IDisposable
    {
        private const int DelayUpdateArModelFrames = 15;

        private readonly SignalBus _signalBus;

        private readonly ArTrackingModel _arTrackingModel;
        private readonly ArComponentsModel _arComponentsModel;
        private readonly ArImageAnchorService _arImageAnchorService;


        private readonly PositionAverageFilterFactory _positionAverageFilterFactory;
        private readonly DirectionAverageFilterFactory _directionAverageFilterFactory;
        private readonly ArImageTrackingConfig _arImageTrackingConfig;


        private readonly Dictionary<string, int> _updatePositionElapsedFrames = new();
        private readonly Dictionary<string, Vector3AverageFilter> _positions = new();
        private readonly Dictionary<string, Vector3AverageFilter> _forwardDirections = new();
        private readonly Dictionary<string, Vector3AverageFilter> _upDirections = new();

        private IDisposable _arReadyStream;
        private IDisposable _resetTrackingStream;

        public int GetPositionsCountByImageId(string imageId)
        {
            _updatePositionElapsedFrames.TryGetValue(imageId, out var result);
            return result;
        }

        public ArImageTrackingService(SignalBus signalBus,
            ArTrackingModel arTrackingModel,
            ArComponentsModel arComponentsModel,
            ArImageAnchorService arImageAnchorService,
            PositionAverageFilterFactory positionAverageFilterFactory,
            DirectionAverageFilterFactory directionAverageFilterFactory,
            ArImageTrackingConfig arImageTrackingConfig)
        {
            _arTrackingModel = arTrackingModel;
            _arComponentsModel = arComponentsModel;
            _arImageAnchorService = arImageAnchorService;
            _signalBus = signalBus;
            _positionAverageFilterFactory = positionAverageFilterFactory;
            _directionAverageFilterFactory = directionAverageFilterFactory;
            _arImageTrackingConfig = arImageTrackingConfig;
        }

        public void Initialize()
        {
            _resetTrackingStream = _signalBus
                .GetStream<ArSignals.ResetTracking>()
                .Subscribe(signal =>
                {
                    _arTrackingModel.UpdateIsTracked(false);
                    ClearBuffers();
                });
        }

        public void Dispose()
        {
            _arReadyStream?.Dispose();
            _resetTrackingStream?.Dispose();
            Disable();
        }

        public void Enable()
        {
            if (_arTrackingModel.IsActive) return;

            if (!_arComponentsModel.IsReady) return;

            _arTrackingModel.Enable();
            _arTrackingModel.UpdateIsTracked(false);

            _arReadyStream = _arComponentsModel
                .GetIsReadyAsObservable()
                .Subscribe(value =>
                    {
                        if (value)
                        {
                            _arComponentsModel.ArTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
                            return;
                        }
                        
                        Disable();
                    }
                );
        }

        public void Disable()
        {
            _arReadyStream?.Dispose();
            _arTrackingModel.Disable();
            if (_arComponentsModel.IsReady)
            {
                _arComponentsModel.ArTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
            }

            ClearBuffers();
        }

        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var trackedImage in eventArgs.added)
            {
                // Debug.Log($"[ArImageTrackingController] Added: {trackedImage.referenceImage.name}. State: {trackedImage.trackingState}");
                ProcessTrackedImage(trackedImage);
            }

            foreach (var trackedImage in eventArgs.updated)
            {
                // Debug.Log($"[ArImageTrackingController] Updated: {trackedImage.referenceImage.name}. State: {trackedImage.trackingState}");
                ProcessTrackedImage(trackedImage);
            }

            foreach (var trackedImage in eventArgs.removed)
            {
                // Debug.Log($"[ArImageTrackingController] Removed: {trackedImage.referenceImage.name}. State: {trackedImage.trackingState}");
                ProcessTrackedImage(trackedImage);
                RemoveFromBuffers(trackedImage.referenceImage.name);
            }
        }

        private void ProcessTrackedImage(ARTrackedImage trackedImage)
        {
            if (!_arTrackingModel.IsActive) return;

            var anchorId = trackedImage.referenceImage.name;
            if (!_arImageAnchorService.CheckExistAnchorForImage(anchorId))
            {
                Debug.LogError($"[ArImageTrackingController] Anchor for image: {anchorId} not found.");
                return;
            }

            if (trackedImage.trackingState != TrackingState.Tracking)
            {
                _updatePositionElapsedFrames[anchorId] = 0;
                return;
            }

            AddNewToBuffers(anchorId);
            
            _updatePositionElapsedFrames[anchorId]++;
            
            if (_updatePositionElapsedFrames[anchorId] <
                _arImageTrackingConfig.Data.SkipFirstTrackingFrames) return;

            var isNeedToUpdatePosition = _positions[anchorId]
                .AddAndCheckValueIsNewTarget(trackedImage.transform.position);

            var isNeedToUpdateRotation = _forwardDirections[anchorId]
                .AddAndCheckValueIsNewTarget(trackedImage.transform.forward);

            isNeedToUpdateRotation |= _upDirections[anchorId]
                .AddAndCheckValueIsNewTarget(trackedImage.transform.up);

            if (!TryGetNewPositionData(isNeedToUpdatePosition, isNeedToUpdateRotation, anchorId,
                    out var positionData)) return;
            
            UpdateArTrackingModel(true, positionData, anchorId);
        }

        private void AddNewToBuffers(string anchorId)
        {
            if (!_updatePositionElapsedFrames.ContainsKey(anchorId))
            {
                _updatePositionElapsedFrames.Add(anchorId, 0);
            }

            if (!_positions.ContainsKey(anchorId))
            {
                _positions.Add(anchorId, _positionAverageFilterFactory.Create());
            }

            if (!_forwardDirections.ContainsKey(anchorId))
            {
                _forwardDirections.Add(anchorId, _directionAverageFilterFactory.Create());
            }

            if (!_upDirections.ContainsKey(anchorId))
            {
                _upDirections.Add(anchorId, _directionAverageFilterFactory.Create());
            }
        }

        private void RemoveFromBuffers(string anchorId)
        {
            if (!_updatePositionElapsedFrames.ContainsKey(anchorId))
            {
                _updatePositionElapsedFrames.Remove(anchorId);
            }

            if (!_positions.ContainsKey(anchorId))
            {
                _positions.Remove(anchorId);
            }

            if (!_forwardDirections.ContainsKey(anchorId))
            {
                _forwardDirections.Remove(anchorId);
            }

            if (!_upDirections.ContainsKey(anchorId))
            {
                _upDirections.Remove(anchorId);
            }
        }

        private void ClearBuffers()
        {
            foreach (var item in _positions)
            {
                item.Value.Dispose();
            }

            _positions.Clear();


            foreach (var item in _forwardDirections)
            {
                item.Value.Dispose();
            }

            _forwardDirections.Clear();


            foreach (var item in _upDirections)
            {
                item.Value.Dispose();
            }

            _upDirections.Clear();
        }

        private bool TryGetNewPositionData(bool isNeedToUpdatePosition, bool isNeedToUpdateRotation, string anchorId,
            out PositionData positionData)
        {
            positionData = default;
            if (_arTrackingModel.ImageName != anchorId)
            {
                isNeedToUpdatePosition = isNeedToUpdateRotation = true;
            }
            
            switch (_arImageTrackingConfig.Data.ImageTrackingModeType)
            {
                case ImageTrackingModeType.CheckPositionUpdate:

                    if (!isNeedToUpdatePosition) return false;
                    positionData.Position = _positions[anchorId].GetAverage();
                    positionData.Rotation = Quaternion.LookRotation(_forwardDirections[anchorId].GetAverage(),
                        _upDirections[anchorId].GetAverage());
                    return true;
                case ImageTrackingModeType.CheckPositionAndRotationUpdate:

                    if (isNeedToUpdatePosition && isNeedToUpdateRotation)
                    {
                        positionData.Position = _positions[anchorId].GetAverage();
                        positionData.Rotation = Quaternion.LookRotation(_forwardDirections[anchorId].GetAverage(),
                            _upDirections[anchorId].GetAverage());
                        return true;
                    }

                    if (isNeedToUpdateRotation)
                    {
                        positionData.Position = _arTrackingModel.GetPosition().Position;
                        positionData.Rotation = Quaternion.LookRotation(_forwardDirections[anchorId].GetAverage(),
                            _upDirections[anchorId].GetAverage());
                        return true;
                    }

                    if (isNeedToUpdatePosition)
                    {
                        positionData.Position = _positions[anchorId].GetAverage();
                        positionData.Rotation = _arTrackingModel.GetPosition().Rotation;
                        return true;
                    }

                    return false;
                default:
                    return false;
            }
        }

        private async void UpdateArTrackingModel(bool value, PositionData positionData, string imageName)
        {
            if (_arTrackingModel.ImageName != imageName && value)
            {
                _arTrackingModel.UpdateIsTracked(false);

                await UniTask.DelayFrame(DelayUpdateArModelFrames);

                if (_arImageAnchorService.TryGetImageAnchorContentBoundPositions(imageName,
                        out var contentBoundPositions))
                {
                    _arTrackingModel.UpdateContentBoundPositions(contentBoundPositions);
                }
            }

            _arTrackingModel.UpdateIsTracked(value, positionData, imageName);
        }

       
    }
}