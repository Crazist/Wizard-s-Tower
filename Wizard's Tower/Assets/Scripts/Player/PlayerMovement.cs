using Factory;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _maxMoveSpeed = 1.2f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _rotationSpeed = 500f;
        [SerializeField] private float _turnSpeedReduction = 0.5f;
        [SerializeField] private float _turnThreshold = 15f;
        [SerializeField] private float _accelerationSpeed = 5f;
        [SerializeField] private float _decelerationSpeed = 5f;
        [SerializeField] private CharacterController _controller;
        [SerializeField] private AnimationController _animationController;

        private Vector3 _velocity;
        private UIFactory _uiFactory;
        private Transform _playerTransform;
        private float _currentMoveSpeed;
        private float _turnInput;

        private bool _isMovementStopped;

        [Inject]
        private void Construct(UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _playerTransform = transform;
            _currentMoveSpeed = 0f;
        }

        private void Update()
        {
            if (_isMovementStopped) return;

            HandleMovement();
        }

        public void StopMovement()
        {
            _isMovementStopped = true;
            StopAllMovement();
        }

        public void ResumeMovement()
        {
            _isMovementStopped = false;
            _animationController.PlayMovementAnimation();
        }

        private void HandleMovement()
        {
            if (!IsInputAvailable()) return;

            Vector3 moveDirection = GetInputDirection();
            float moveMagnitude = GetInputMagnitude();

            if (IsMovementBelowThreshold(moveMagnitude)) return;

            AdjustSpeed(moveMagnitude, moveDirection);
            HandleRotation(moveDirection, moveMagnitude);
            ApplyGravity();
            PerformMovement(moveDirection);
        }

        private bool IsInputAvailable()
        {
            return _uiFactory.FixedJoystick != null;
        }

        private Vector3 GetInputDirection()
        {
            float moveX = _uiFactory.FixedJoystick.Horizontal;
            float moveZ = _uiFactory.FixedJoystick.Vertical;
            return new Vector3(moveX, 0f, moveZ).normalized;
        }

        private float GetInputMagnitude()
        {
            float moveX = _uiFactory.FixedJoystick.Horizontal;
            float moveZ = _uiFactory.FixedJoystick.Vertical;
            return new Vector2(moveX, moveZ).magnitude;
        }

        private bool IsMovementBelowThreshold(float moveMagnitude)
        {
            if (moveMagnitude < 0.1f)
            {
                StopAllMovement();
                return true;
            }
            else
            {
                _animationController.PlayMovementAnimation();
            }

            return false;
        }

        private void StopAllMovement()
        {
            _currentMoveSpeed = 0f;
            _animationController.StopMovementAnimation();
        }

        private void AdjustSpeed(float moveMagnitude, Vector3 moveDirection)
        {
            float targetSpeed = _maxMoveSpeed * moveMagnitude;
            bool isTurning = IsTurning(moveDirection);

            if (isTurning) targetSpeed *= (1f - _turnSpeedReduction);

            _currentMoveSpeed = Mathf.Lerp(
                _currentMoveSpeed,
                targetSpeed,
                (isTurning ? _decelerationSpeed : _accelerationSpeed) * Time.deltaTime
            );

            _turnInput = GetTurnInput(moveDirection);
            _animationController.UpdateMovementAnimation(_currentMoveSpeed / _maxMoveSpeed, _turnInput);
        }

        private bool IsTurning(Vector3 moveDirection)
        {
            float angleDifference = Vector3.SignedAngle(_playerTransform.forward, moveDirection, Vector3.up);
            return Mathf.Abs(angleDifference) > _turnThreshold;
        }

        private float GetTurnInput(Vector3 moveDirection)
        {
            float angleDifference = Vector3.SignedAngle(_playerTransform.forward, moveDirection, Vector3.up);
            return Mathf.Clamp(angleDifference / 90f, -1f, 1f);
        }

        private void HandleRotation(Vector3 moveDirection, float moveMagnitude)
        {
            if (moveMagnitude >= 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                _playerTransform.rotation = Quaternion.RotateTowards(
                    _playerTransform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime
                );
            }
        }

        private void ApplyGravity()
        {
            if (_controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            _velocity.y += _gravity * Time.deltaTime;
        }

        private void PerformMovement(Vector3 moveDirection)
        {
            Vector3 movement = moveDirection * (_currentMoveSpeed * Time.deltaTime);
            _controller.Move(movement);
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}