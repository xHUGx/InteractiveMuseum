using System;

namespace Features.ScenePlayer.Handlers
{
    public interface IPlaySceneHandler : IDisposable
    {
        public void Initialize(string sceneName);
    }
}
