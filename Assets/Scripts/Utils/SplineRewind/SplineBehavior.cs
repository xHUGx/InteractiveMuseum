using UnityEngine;
using UnityEngine.Playables;

namespace Utils.SplineRewind
{
    public class SplineBehavior : PlayableBehaviour
    {
        public Transform startLocation;
        public Transform endLocation;

        public bool shouldSplinePosition;
        public bool shouldSplineRotation;

        public AnimationCurve curve;
    }
}
