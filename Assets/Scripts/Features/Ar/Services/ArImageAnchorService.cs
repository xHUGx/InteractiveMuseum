using System;
using System.Collections.Generic;
using Features.Ar.Data;
using Features.Ar.Messages;
using UniRx;
using UnityEngine;
using Zenject;

namespace Features.Ar.Services
{
    public class ArImageAnchorService : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;

        private readonly Dictionary<string, ArImageAnchorData> _anchors;

        private readonly CompositeDisposable _compositeDisposable;

        public ArImageAnchorService(SignalBus signalBus)
        {
            _signalBus = signalBus;

            _anchors = new Dictionary<string, ArImageAnchorData>();
            _compositeDisposable = new CompositeDisposable();
        }
        public bool CheckExistAnchorForImage(string id)
        {
            return _anchors.ContainsKey(id);
        }
        public bool TryGetImageAnchorTransform(string id, out Transform transform)
        {
            transform = null;
            if (!_anchors.TryGetValue(id, out var arImageAnchorData)) return false;
            transform = arImageAnchorData.Transform;
            return true;
        }

        public bool TryGetImageAnchorContentBoundPositions(string id, out Vector3[] contentBoundPositions)
        {
            contentBoundPositions = null;
            if (!_anchors.TryGetValue(id, out var arImageAnchorData)) return false;
            contentBoundPositions = arImageAnchorData.ContentBoundPositions;
            return true;
        }

        public void Initialize()
        {
            _signalBus
                .GetStream<ArSignals.RegisterNewImageAnchor>()
                .Subscribe(signal =>
                {
                    var newArImageAnchorData =
                        new ArImageAnchorData(signal.Transform, signal.ContentBoundPositions);

                    _anchors.TryAdd(signal.Id, newArImageAnchorData);
                    // Debug.Log($"[ImageAnchorService] New image anchor added. Id: {signal.Id}");
                })
                .AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _anchors?.Clear();
            _compositeDisposable.Dispose();
        }
    }
}