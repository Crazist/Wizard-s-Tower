using Factory;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _maxMoveSpeed = 1.2f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _rotationSpeed = 500f; // Скорость поворота
        [SerializeField] private float _turnSpeedReduction = 0.5f; // Уменьшение скорости во время поворота
        [SerializeField] private float _turnThreshold = 15f; // Порог для активации анимации поворота
        [SerializeField] private float _accelerationSpeed = 5f; // Скорость ускорения
        [SerializeField] private float _decelerationSpeed = 5f; // Скорость замедления
        [SerializeField] private CharacterController _controller;
        [SerializeField] private AnimationController _animationController;

        private Vector3 _velocity;
        private UIFactory _uiFactory;
        private Transform _playerTransform;
        private float _currentMoveSpeed; // Текущая скорость
        private float _turnInput; // Параметр для поворота
        private bool _isMovementStopped;

        [Inject]
        public void Construct(UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _playerTransform = transform;
            _currentMoveSpeed = 0f; // Начальная скорость
        }

        private void Update()
        {
            if (!_isMovementStopped)
            {
                Move();
            }
        }

        private void Move()
        {
            if (_uiFactory.FixedJoystick == null) return;

            float moveX = _uiFactory.FixedJoystick.Horizontal;
            float moveZ = _uiFactory.FixedJoystick.Vertical;
            Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

            float moveMagnitude = new Vector2(moveX, moveZ).magnitude;
            float targetSpeed = _maxMoveSpeed * moveMagnitude;

            float angleDifference = Vector3.SignedAngle(_playerTransform.forward, moveDirection, Vector3.up);
            bool isTurning = Mathf.Abs(angleDifference) > _turnThreshold;

            if (isTurning && moveMagnitude >= 0.1f)
            {
                targetSpeed *= (1f - _turnSpeedReduction);
            }
            _currentMoveSpeed = Mathf.Lerp(_currentMoveSpeed, targetSpeed, (isTurning ? _decelerationSpeed : _accelerationSpeed) * Time.deltaTime);

            _turnInput = Mathf.Clamp(angleDifference / 90f, -1f, 1f);
            _animationController.UpdateMovementAnimation(_currentMoveSpeed / _maxMoveSpeed, _turnInput);

            if (_controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            _velocity.y += _gravity * Time.deltaTime;

            if (moveMagnitude >= 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                _playerTransform.rotation = Quaternion.RotateTowards(
                    _playerTransform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime
                );
            }

            Vector3 movement = moveDirection * (_currentMoveSpeed * Time.deltaTime);
            _controller.Move(movement);

            _controller.Move(_velocity * Time.deltaTime);
        }

        public void StopMovement()
        {
            _isMovementStopped = true;
            _currentMoveSpeed = 0f;
            _animationController.StopMovementAnimation();
        }

        public void ResumeMovement()
        {
            _isMovementStopped = false;
            _animationController.PlayMovementAnimation();
        }
    }
}
