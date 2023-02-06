namespace Features.ScenePlayer.Views
{
    public interface IScenePlayer
    {
        public string SceneName { get; }
        public void PlayScene();
        public void StopScene();
    }
}