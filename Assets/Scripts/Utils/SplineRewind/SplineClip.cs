using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Utils.SplineRewind
{
    [Serializable]
    [DisplayName("Spline Clip")]
    public class SplineClip :  PlayableAsset, ITimelineClipAsset, IPropertyPreview
    {
        public ExposedReference<Transform> startLocation;
        public ExposedReference<Transform> endLocation;

        [Tooltip("Changes the position of the assigned object")]
        public bool shouldSplinePosition = true;

        [Tooltip("Changes the rotation of the assigned object")]
        public bool shouldSplineRotation = true;

        [Tooltip("Only keys in the [0,1] range will be used")]
        public AnimationCurve curve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
        
        // Implementation of ITimelineClipAsset. This specifies the capabilities of this timeline clip inside the editor.
        public ClipCaps clipCaps
        {
            get { return ClipCaps.Blending; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            // create a new SplineBehaviour
            ScriptPlayable<SplineBehavior> playable = ScriptPlayable<SplineBehavior>.Create(graph);
            SplineBehavior spline = playable.GetBehaviour();

            // set the behaviour's data
            spline.startLocation = startLocation.Resolve(graph.GetResolver());
            spline.endLocation = endLocation.Resolve(graph.GetResolver());
            spline.curve = curve;
            spline.shouldSplinePosition = shouldSplinePosition;
            spline.shouldSplineRotation = shouldSplineRotation;

            return playable;
        }

      
        public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            const string kLocalPosition = "m_LocalPosition";
            const string kLocalRotation = "m_LocalRotation";

            driver.AddFromName<Transform>(kLocalPosition + ".x");
            driver.AddFromName<Transform>(kLocalPosition + ".y");
            driver.AddFromName<Transform>(kLocalPosition + ".z");
            driver.AddFromName<Transform>(kLocalRotation + ".x");
            driver.AddFromName<Transform>(kLocalRotation + ".y");
            driver.AddFromName<Transform>(kLocalRotation + ".z");
            driver.AddFromName<Transform>(kLocalRotation + ".w");
        }
    }
}
