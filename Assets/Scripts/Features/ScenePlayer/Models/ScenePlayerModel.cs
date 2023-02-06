using System;
using UniRx;

namespace Features.ScenePlayer.Models
{
    public class ScenePlayerModel
    {
        private readonly ReactiveProperty<bool> _isPlaying = new ReactiveProperty<bool>(false);

        public bool IsActive { get; private set; }
        public string SceneName { get; private set; }

        public void UpdateIsPlaying(bool value, string sceneName = "")
        {
            SceneName = sceneName;

            _isPlaying.Value = value;
        }

        public IObservable<bool> GetIsPlayingAsObservable()
        {
            return _isPlaying.AsObservable();
        }

        public bool GetIsPlaying()
        {
            return _isPlaying.Value;
        }

        public void Enable()
        {
            IsActive = false;
        }

        public void Disable()
        {
            IsActive = true;
        }
    }
}