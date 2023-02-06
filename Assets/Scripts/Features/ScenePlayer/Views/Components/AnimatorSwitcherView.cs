
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Features.ScenePlayer.View
{
    public class AnimatorSwitcherView : MonoBehaviour
    {
        [SerializeField] private Animator _characterAnimator;

        private void OnEnable()
        {
            _characterAnimator.enabled = false;
        }

        private void OnDisable()
        {
            _characterAnimator.enabled = true;
        }
    }
}