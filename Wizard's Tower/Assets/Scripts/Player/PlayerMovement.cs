using Factory;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _maxMoveSpeed = 1.2f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private CharacterController _controller;

        private Vector3 _velocity;
        private UIFactory _uiFactory;
        private Transform _playerTransform;

        [Inject]
        public void Construct(UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _playerTransform = transform;
        }

        private void Update() => Move();

        private void Move()
        {
            if (_uiFactory.FixedJoystick == null) return;

            float moveX = _uiFactory.FixedJoystick.Horizontal;
            float moveZ = _uiFactory.FixedJoystick.Vertical;

            Vector3 move = new Vector3(moveX, 0f, moveZ).normalized;

            float moveMagnitude = new Vector2(moveX, moveZ).magnitude;
        
            float currentMoveSpeed = _maxMoveSpeed * moveMagnitude;

            if (_controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            _velocity.y += _gravity * Time.deltaTime;

            if (moveMagnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
                _playerTransform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

                _controller.Move(move * (currentMoveSpeed * Time.deltaTime));
            }

            _controller.Move(_velocity * Time.deltaTime);
        }

        public void StopMovement()
        {
          //  throw new System.NotImplementedException();
        }

        public void ResumeMovement()
        {
         //   throw new System.NotImplementedException();
        }
    }
}
