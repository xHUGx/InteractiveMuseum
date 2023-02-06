#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Utils.Editor
{
    public static class EditorMenuExtension
    {
        private const string StartupSceneName = "Startup.unity";
        private const string ScenePlayerSceneName = "ScenePlayer.unity";
        private const string PathToScenes = "Assets/Scenes/";
        
        private static Vector3 _position;
        private static Quaternion _rotation;
        private static Vector3 _scale;
        
        [MenuItem("Tools/OpenScene '" + StartupSceneName + "' _#F5")]
        private static void OpenStartupScene()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorSceneManager.OpenScene(PathToScenes + StartupSceneName);
            }
        }
        
        [MenuItem("Tools/OpenScene '" + ScenePlayerSceneName + "' _#F6")]
        private static void OpenScenePlayerScene()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorSceneManager.OpenScene(PathToScenes + ScenePlayerSceneName);
            }
        }

        
        
        [MenuItem("Tools/CopyTransform _#&C")]
        static void CopyTransform()
        {
            if (Selection.activeGameObject != null)
            {
                _position = Selection.activeGameObject.transform.position;
                _rotation = Selection.activeGameObject.transform.rotation;
                _scale = Selection.activeGameObject.transform.localScale;
            }
        }

        [MenuItem("Tools/PasteTransform _#&V")]
        static void PasteTransform()
        {
            if (Selection.activeGameObject != null)
            {
                Undo.RecordObject(Selection.activeGameObject.transform, "Paste transform values");
                Selection.activeGameObject.transform.position = _position;
                Selection.activeGameObject.transform.rotation = _rotation;
                Selection.activeGameObject.transform.localScale = _scale;
            }
        }
    }
}
#endif