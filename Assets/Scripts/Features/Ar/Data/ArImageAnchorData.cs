using UnityEngine;

namespace Features.Ar.Data
{
    public class ArImageAnchorData
    {
        public Transform Transform;
        public Vector3[] ContentBoundPositions;

        public ArImageAnchorData(Transform transform, Vector3[] contentBoundPositions)
        {
            Transform = transform;
            ContentBoundPositions = contentBoundPositions;
        }
    }
}