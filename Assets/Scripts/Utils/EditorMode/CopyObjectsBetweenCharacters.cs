using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace icvr.utils
{
    [ExecuteInEditMode]
    public class CopyObjectsBetweenCharacters : MonoBehaviour
    {
        [SerializeField] private Transform _from;
        [SerializeField] private Transform _to;

        private const char EffectPrefix = '-';

        [ContextMenu("CopyTransforms")]
        private void CopyTransforms()
        {
            var fromTransforms = _from.GetComponentsInChildren<Transform>();
            var toTransforms = _to.GetComponentsInChildren<Transform>();

            for (var i = 0; i < fromTransforms.Length; i++)
            {
                var transform = toTransforms.FirstOrDefault(t => t.name == fromTransforms[i].name);

                Debug.Log(fromTransforms[i].name + " : " + transform);

                if (transform != null)
                {
                    transform.localPosition = fromTransforms[i].localPosition;
                    transform.localRotation = fromTransforms[i].localRotation;
                }
            }
        }

        [ContextMenu("CopyEffects")]
        private void CopyEffects()
        {
            var fromTransforms = _from.GetComponentsInChildren<Transform>();
            var toTransforms = _to.GetComponentsInChildren<Transform>();

            for (var i = 0; i < fromTransforms.Length; i++)
            {
                if (!fromTransforms[i].name[0].Equals(EffectPrefix))
                    continue;
                
                var parent = toTransforms.FirstOrDefault(t => t.name.Equals(fromTransforms[i].parent.name));

                if (parent != null)
                {
                    var effect = GameObject.Instantiate(fromTransforms[i].gameObject, parent) as GameObject;

                    Debug.Log("##: " + effect);

                    effect.name = fromTransforms[i].name;
                    effect.transform.position = fromTransforms[i].transform.position;
                    effect.transform.rotation = fromTransforms[i].transform.rotation;
                }

            }
        }

    }
}
#endif