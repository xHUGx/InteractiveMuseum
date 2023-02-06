using System.Linq;
using Zenject;

// ReSharper disable once CheckNamespace
namespace Bootstrap
{
    public class UpdateProjectContextSceneContext: SceneContext
    {
        protected void Awake()
        {
            // Debug.Log($"[UpdateProjectContextSceneContext] Awake");
            ProjectContext.PreInstall += () =>
            {
                var installers = gameObject.GetComponents<MonoInstaller>();
                if (installers.Length == 0) return;
                var currentInstallers = ProjectContext.Instance.Installers.ToList();
                currentInstallers.AddRange(installers);
                // Debug.Log($"[UpdateProjectContextSceneContext] Update installers");
                ProjectContext.Instance.Installers = currentInstallers;
            };
            
            base.Awake();
        }
    }
}