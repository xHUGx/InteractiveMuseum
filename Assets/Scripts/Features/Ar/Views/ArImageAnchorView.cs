using Features.Ar.Messages;
using UnityEngine;
using Zenject;

namespace Features.Ar.Views
{
    public class ArImageAnchorView : MonoBehaviour
    {
        [SerializeField] private string imageName;
        [SerializeField] private Transform[] _contentBoundPositions;
        private SignalBus _signalBus;
        public string ImageName => imageName;
        public Transform Transform => transform;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            Initialize();
        }

        private void Initialize()
        {
            var positions = new Vector3[_contentBoundPositions.Length];
            for (int i = 0; i < _contentBoundPositions.Length; i++)
            {
                positions[i] = _contentBoundPositions[i].localPosition;
            }

            _signalBus.TryFire(new ArSignals.RegisterNewImageAnchor(ImageName, Transform, positions));
        }
    }
}