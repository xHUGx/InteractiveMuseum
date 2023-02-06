using Cysharp.Threading.Tasks;
using Features.DebugSystem.Models;
using Features.DebugSystem.Models;
using UnityEngine;
using Zenject;
using UniRx;

namespace Features.UI.View
{
    public class DebugGraphicsView : MonoBehaviour
    {
        private const int DelayBeforeInitializeMs = 500;
        private IDebugGraphicsProvider _debugGraphicsProvider;

        [Inject]
        public async void Construct([InjectOptional] IDebugGraphicsProvider debugGraphicsProvider)
        {
            _debugGraphicsProvider = debugGraphicsProvider;
            await UniTask.Delay(DelayBeforeInitializeMs);
            Initialize();
        }

        private void Initialize()
        {
            if (_debugGraphicsProvider == null)
            {
                Dispose();
                return;
            }

            _debugGraphicsProvider
                .GetIsEnabledAsObservable()
                .Subscribe(value => gameObject.SetActive(value))
                .AddTo(this);
        }


        private void Dispose()
        {
            try
            {
                Destroy(gameObject);
            }
            catch
            {
                // Ignored
            }
        }
    }
}