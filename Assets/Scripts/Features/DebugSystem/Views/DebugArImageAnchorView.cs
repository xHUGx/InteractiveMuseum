#if UNITY_EDITOR
using Features.Ar.Attributes;
#endif
using UnityEngine;

namespace Features.UI.View
{
    public class DebugArImageAnchorView: MonoBehaviour
    {
#if UNITY_EDITOR
        [AttributeArImageTypeName]
#endif
        [SerializeField] private string _imageName;
        public string ImageName => _imageName;
        public Transform Transform => transform;
    }
}