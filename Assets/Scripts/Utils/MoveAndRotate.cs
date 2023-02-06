using System;
using UnityEngine;

namespace Utils
{
    public class MoveAndRotate : MonoBehaviour
    {
        [SerializeField] private Vector3andSpace _moveUnitsPerSecond;
        [SerializeField] private Vector3andSpace _rotateDegreesPerSecond;
        [SerializeField] private bool _ignoreTimescale;
        private float _mLastRealTime;

        private void Start()
        {
            _mLastRealTime = Time.realtimeSinceStartup;
        }
        
        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;
            if (_ignoreTimescale)
            {
                deltaTime = (Time.realtimeSinceStartup - _mLastRealTime);
                _mLastRealTime = Time.realtimeSinceStartup;
            }
            transform.Translate(_moveUnitsPerSecond.value*deltaTime, _moveUnitsPerSecond.space);
            transform.Rotate(_rotateDegreesPerSecond.value*deltaTime, _rotateDegreesPerSecond.space);
        }


        [Serializable]
        public class Vector3andSpace
        {
            public Vector3 value;
            public Space space = Space.Self;
        }
    }
}
