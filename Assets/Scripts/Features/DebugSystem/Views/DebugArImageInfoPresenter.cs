using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Features.DebugSystem.Models;

namespace Features.DebugSystem.Views
{
    public class DebugArImageInfoPresenter : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject planeGameObject;
        [SerializeField] private Text text;
    
        public Canvas Canvas => canvas;
        public GameObject PlaneGameObject => planeGameObject;
        public Text Text => text;
        public Transform Transform => transform;

        private IDebugArImageInfoEnabledProvider _enabledProvider;
        private bool _isEnabled;
        private bool _isVisible;
        [Inject]
        public void Construct(IDebugArImageInfoEnabledProvider enabledProvider)
        {
            _enabledProvider = enabledProvider;
        }

        public void Initialize()
        {
            _enabledProvider
                .GetIsEnabledImageInfoAsObservable()
                .Subscribe(value =>
                {
                    _isEnabled = value;
                    UpdateVisible();
                })
                .AddTo(this);
        }

        private void UpdateVisible()
        {
            gameObject.SetActive(_isEnabled);
        }
        public void Dispose()
        {
            Destroy(gameObject);
        }


        public void Show()
        {
            UpdateVisible();
            // _isVisible = true;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            // _isVisible = false;
            // UpdateVisible();
        }
    }
}