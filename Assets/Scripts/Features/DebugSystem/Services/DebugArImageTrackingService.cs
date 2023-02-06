using System;
using System.Collections.Generic;
using Features.Ar.Models;
using Features.DebugSystem.Factories;
using Features.DebugSystem.Views;
using UniRx;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Features.DebugSystem.Services
{
    public class DebugArImageTrackingService : IDisposable
    {
        private readonly DebugArImageInfoPresenterFactory _debugArImageInfoPresenterFactory;
        private readonly ArComponentsModel _arComponentsModel;
        
        private readonly Dictionary<string, DebugArImageInfoPresenter> _presenters = new ();

        
        private IDisposable _arReadyStream;

        public DebugArImageTrackingService(DebugArImageInfoPresenterFactory debugArImageInfoPresenterFactory, ArComponentsModel arComponentsModel)
        {
            _debugArImageInfoPresenterFactory = debugArImageInfoPresenterFactory;
            _arComponentsModel = arComponentsModel;
        }

        public void Enable()
        {
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
            if (_arComponentsModel.IsReady)
            {
                _arComponentsModel.ArTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
            }
            // HidePresenters();
        }
        
        private void ClearPresenters()
        {
            foreach (var item in _presenters)
            {
                if (item.Value != null)
                    item.Value.Dispose();
            }

            _presenters.Clear();
        }
        
        private void HidePresenters()
        {
            foreach (var item in _presenters)
            {
                if (item.Value != null)
                    item.Value.Hide();
            }
        }
        
        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var trackedImage in eventArgs.added)
            {
                if (!_presenters.TryGetValue(trackedImage.referenceImage.name, out var presenter))
                {
                    presenter = _debugArImageInfoPresenterFactory.Create(trackedImage.transform);
                    _presenters.Add(trackedImage.referenceImage.name, presenter);
                }

                UpdatePresenter(trackedImage, presenter);

            }

            foreach (var trackedImage in eventArgs.updated)
            {
                if (_presenters.TryGetValue(trackedImage.referenceImage.name, out var presenter))
                {
                    UpdatePresenter(trackedImage, presenter);
                }
            }

            foreach (var trackedImage in eventArgs.removed)
            {
                var id = trackedImage.referenceImage.name;

                if (_presenters.ContainsKey(id))
                {
                    _presenters[id].Dispose();
                    _presenters.Remove(id);
                }
            }
        }
        
        private void UpdatePresenter(ARTrackedImage trackedImage, DebugArImageInfoPresenter presenter)
        {
            if (presenter == null) return;

            presenter.Canvas.worldCamera = _arComponentsModel.CameraView.Camera;
            presenter.Text.text =
                $"Image name:{trackedImage.referenceImage.name}\n" +
                $"Tracking state: {trackedImage.trackingState}\n" +
                $"GUID: {trackedImage.referenceImage.guid}\n" +
                $"Reference size: {trackedImage.referenceImage.size * 100f} cm\n" +
                $"Detected size: {trackedImage.size * 100f} cm";

            presenter.Transform.localScale = new Vector3(trackedImage.size.x, 1f, trackedImage.size.y);


            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                presenter.Show();
                return;
            }
            
            presenter.Hide();
        }

        public void Dispose()
        {
            ClearPresenters();
        }
    }
}