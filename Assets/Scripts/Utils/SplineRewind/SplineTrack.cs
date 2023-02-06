using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Utils.SplineRewind;

[TrackColor(1.0f, 0.8f, 0.8f)]
[TrackBindingType(typeof(Transform))]
[TrackClipType(typeof(SplineClip))]
    public class SplineTrack : TrackAsset
    {
        // Creates a runtime instance of the track, represented by a PlayableBehaviour.
        // The runtime instance performs mixing on the clips.
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<SplineMixerBehavior>.Create(graph, inputCount);
        }
    }

