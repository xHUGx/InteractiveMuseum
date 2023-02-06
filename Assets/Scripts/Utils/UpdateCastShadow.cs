using UnityEngine;
using UnityEngine.Rendering;
namespace Utils
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    public class UpdateCastShadow : MonoBehaviour
    {
        [SerializeField] private bool execute = false;
        [SerializeField] private ShadowCastingMode newValue = ShadowCastingMode.Off;
        private void Update()
        {
            if (execute)
            {
                execute = false;
                var allGameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>(true);
                foreach (var obj in allGameObjects)
                {
                    var renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                        if (renderer.shadowCastingMode != newValue)
                        {
                            Debug.Log($"Value updated. Object: {obj.name}");
                            renderer.shadowCastingMode = newValue;
                        }
                }
            }
        }
    }
#endif
}