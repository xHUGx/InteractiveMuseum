#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Utils.Editor
{
    [InitializeOnLoad]
    public class OnUnityLoad
    {
        static OnUnityLoad()
        {
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {

                Debug.Log("Auto-Saving before entering Play mode");
                EditorSceneManager.MarkAllScenesDirty();
                EditorSceneManager.SaveOpenScenes();
                AssetDatabase.SaveAssets();
                EditorApplication.ExecuteMenuItem("File/Save Project");
            }
        }
    }
}
#endif