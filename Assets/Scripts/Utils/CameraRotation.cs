using System;
using UnityEngine;

namespace Utils
{
    public class CameraRotation : MonoBehaviour
    {
        public float _speed = 3.5f;
        private float _rotationX;
        private float _rotationY;

        private Touch initTouch = new Touch();

        void FixedUpdate() {
            /*if(Input.GetMouseButton(0)) {
                transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * _speed, -Input.GetAxis("Mouse X") * _speed, 0));
                _rotationX = transform.rotation.eulerAngles.x;
                _rotationY = transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
            }*/

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    initTouch = touch;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    float deltaX = initTouch.position.x - touch.position.x;
                    float deltaY = initTouch.position.y - touch.position.y;
                    _rotationX -= deltaY * Time.deltaTime * _speed * -1;
                    _rotationY += deltaX * Time.deltaTime * _speed * -1;
                    //_rotationX = Math.Clamp(_rotationX, -45f, -45f);
                    transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    initTouch = new Touch();
                }
            }
            
        }
    }
}
