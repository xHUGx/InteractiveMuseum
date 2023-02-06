using UnityEngine;

namespace Features.Ar.Messages
{
    public static class ArSignals
    {
        // public sealed class ImageFound
        // {
        //     public Transform Anchor { get; }
        //     public string Name { get; }
        //
        //     public ImageFound(Transform anchor, string name)
        //     {
        //         Anchor = anchor;
        //         Name = name;
        //     }
        // }
        
        // public sealed class ImageLost
        // {
        // }
        //
        public sealed class ResetTracking
        {
        }
        
        public sealed class RegisterNewImageAnchor
        {
            public string Id { get; }
            public Transform Transform { get; }
            public Vector3[] ContentBoundPositions { get; }

            public RegisterNewImageAnchor(string id, Transform transform, Vector3[] contentBoundPositions)
            {
                Id = id;
                Transform = transform;
                ContentBoundPositions = contentBoundPositions;
            }
        }
    }
}