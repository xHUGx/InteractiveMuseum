using Timeline.Samples;
using UnityEngine;
using UnityEngine.Playables;

namespace Utils.SplineRewind
{
    public class SplineMixerBehavior : PlayableBehaviour
    {
        static AnimationCurve s_DefaultCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

        bool m_ShouldInitializeTransform = true;
        Vector3 m_InitialPosition;
        Quaternion m_InitialRotation;

        // Performs blend of position and rotation of all clips connected to a track mixer
        // The result is applied to the track binding's (playerData) transform.
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Transform trackBinding = playerData as Transform;

            if (trackBinding == null)
                return;

            // Get the initial position and rotation of the track binding, only when ProcessFrame is first called
            InitializeIfNecessary(trackBinding);

            Vector3 accumPosition = Vector3.zero;
            Quaternion accumRotation = QuaternionUtils.zero;

            float totalPositionWeight = 0.0f;
            float totalRotationWeight = 0.0f;

            // Iterate on all mixer's inputs (ie each clip on the track)
            int inputCount = playable.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                if (inputWeight <= 0)
                    continue;

                Playable input = playable.GetInput(i);
                float normalizedInputTime = (float)(input.GetTime() / input.GetDuration());

                // get the clip's behaviour and evaluate the progression along the curve
                SplineBehavior splineInput = GetSplineBehaviour(input);
                float splineProgress = GetCurve(splineInput).Evaluate(normalizedInputTime);

                // calculate the position's progression along the curve according to the input's (clip) weight
                if (splineInput.shouldSplinePosition)
                {
                    totalPositionWeight += inputWeight;
                    accumPosition += SplinePosition(splineInput, splineProgress, inputWeight);
                }

                // calculate the rotation's progression along the curve according to the input's (clip) weight
                if (splineInput.shouldSplineRotation)
                {
                    totalRotationWeight += inputWeight;
                    accumRotation = SplineRotation(splineInput, accumRotation, splineProgress, inputWeight);
                }
            }

            // Apply the final position and rotation values in the track binding
            trackBinding.position = accumPosition + m_InitialPosition * (1.0f - totalPositionWeight);
            trackBinding.rotation = accumRotation.Blend(m_InitialRotation, 1.0f - totalRotationWeight);
            trackBinding.rotation.Normalize();
        }

        void InitializeIfNecessary(Transform transform)
        {
            if (m_ShouldInitializeTransform)
            {
                m_InitialPosition = transform.position;
                m_InitialRotation = transform.rotation;
                m_ShouldInitializeTransform = false;
            }
        }

        Vector3 SplinePosition(SplineBehavior splineInput, float progress, float weight)
        {
            Vector3 startPosition = m_InitialPosition;
            if (splineInput.startLocation != null)
            {
                startPosition = splineInput.startLocation.position;
            }

            Vector3 endPosition = m_InitialPosition;
            if (splineInput.endLocation != null)
            {
                endPosition = splineInput.endLocation.position;
            }

            return Vector3.Lerp(startPosition, endPosition, progress) * weight;
        }

        Quaternion SplineRotation(SplineBehavior splineInput, Quaternion accumRotation, float progress, float weight)
        {
            Quaternion startRotation = m_InitialRotation;
            if (splineInput.startLocation != null)
            {
                startRotation = splineInput.startLocation.rotation;
            }

            Quaternion endRotation = m_InitialRotation;
            if (splineInput.endLocation != null)
            {
                endRotation = splineInput.endLocation.rotation;
            }

            Quaternion desiredRotation = Quaternion.Lerp(startRotation, endRotation, progress);
            return accumRotation.Blend(desiredRotation.NormalizeSafe(), weight);
        }

        static SplineBehavior GetSplineBehaviour(Playable playable)
        {
            ScriptPlayable<SplineBehavior> splineInput = (ScriptPlayable<SplineBehavior>)playable;
            return splineInput.GetBehaviour();
        }

        static AnimationCurve GetCurve(SplineBehavior spline)
        {
            if (spline == null || spline.curve == null)
                return s_DefaultCurve;
            return spline.curve;
        }
    }
}
