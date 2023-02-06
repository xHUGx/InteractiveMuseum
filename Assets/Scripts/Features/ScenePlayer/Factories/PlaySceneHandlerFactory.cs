using Zenject;

namespace Features.ScenePlayer.Handlers
{
    public class PlaySceneHandlerFactory
    {
        private readonly DiContainer _diContainer;

        private IPlaySceneHandler _playSceneHandler;

        public PlaySceneHandlerFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public IPlaySceneHandler Spawn(string name)
        {
            _playSceneHandler = _diContainer.ResolveId<IPlaySceneHandler>(name);
            _playSceneHandler.Initialize(name);

            return _playSceneHandler;
        }

        public void Despawn()
        {
            _playSceneHandler.Dispose();
        }
    }
}
