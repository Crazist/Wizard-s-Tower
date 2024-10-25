using Panda;
using UnityEngine;

namespace Enemy
{
    public class KnightAIController : EnemyAIBase
    {
        [SerializeField] private CharacterAnimator _characterAnimator;
        [SerializeField] private float _knightWanderRadius = 15f;
        [SerializeField] private float _minMoveTime = 2f;
        [SerializeField] private float _maxMoveTime = 5f;
        [SerializeField] private float _stoppingDistance = 2f;
        [SerializeField] private float _rotationSpeed = 112.5f;
        [SerializeField] private float _rotationTolerance = 1f;

        private Vector3 _targetPosition;
        private float _moveTimer;
        private bool _isRotating;
        private bool _destinationSet;
        private bool _isMoving;

        private void Start()
        {
            if (_navMeshAgent != null && _navMeshAgent.enabled)
            {
                _navMeshAgent.stoppingDistance = _stoppingDistance;
            }
        }

        [Task]
        public void CheckNavMeshAgent()
        {
            if (_navMeshAgent != null && _navMeshAgent.isActiveAndEnabled)
            {
                Task.current.Succeed();
            }
            else
            {
                Task.current.Fail();
            }
        }

        [Task]
        public void FindRandomPosition()
        {
            if (CurrentRoom != null)
            {
                _targetPosition = MovementService.GetRandomPointInRoom(CurrentRoom);
                _isRotating = true;
                _destinationSet = false;
                Task.current.Succeed();
            }
            else
            {
                Task.current.Fail();
            }
        }

        [Task]
        public void SetMoveTimer()
        {
            _moveTimer = Random.Range(_minMoveTime, _maxMoveTime);
            Task.current.Succeed();
        }

        [Task]
        public void StopAndPrepareToRotate()
        {
            _navMeshAgent.isStopped = true;
            _characterAnimator.StopMovement();
            _isRotating = true;
            _isMoving = false; 
            _characterAnimator.SetMovement(0, 0, _isMoving);
            Task.current.Succeed();
        }

        [Task]
        public void RotateToTarget()
        {
            if (_targetPosition != Vector3.zero && _isRotating)
            {
                Vector3 direction = (_targetPosition - transform.position).normalized;
                direction.y = 0;

                // Проверка завершения поворота
                if (Vector3.Angle(transform.forward, direction) <= _rotationTolerance)
                {
                    _isRotating = false;
                    _characterAnimator.SetMovement(0, 0, false);
                    _isMoving = false;
                    Task.current.Succeed(); // Завершаем задачу
                    return;
                }

                // Поворот к цели
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * _rotationSpeed);

                // Устанавливаем анимацию для поворота
                float horizontal = Mathf.Sign(Vector3.Dot(transform.right, direction));
                _characterAnimator.SetMovement(horizontal, 0, true);
                _isMoving = true;
            }
            else
            {
                Task.current.Fail(); // Нет цели или поворот не начат
            }
        }

        [Task]
        public void MoveToTarget()
        {
            if (!_isRotating && _targetPosition != Vector3.zero && !_destinationSet)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_targetPosition);
                _destinationSet = true;
                _isMoving = true;
                _characterAnimator.SetMovement(0, 1, _isMoving);
            }

            if (_destinationSet)
            {
                // Настройка анимации для движения вперед
                Vector3 direction = _navMeshAgent.desiredVelocity.normalized;
                float vertical = Vector3.Dot(transform.forward, direction) > 0 ? 1f : -1f;

                _characterAnimator.SetMovement(0, vertical, _isMoving);

                // Проверка достижения цели
                if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    _characterAnimator.StopMovement();
                    _destinationSet = false;
                    _isMoving = false;
                    Task.current.Succeed(); // Цель достигнута
                }
            }
            else
            {
                Task.current.Fail(); // Поворот не завершен или нет цели
            }
        }

        [Task]
        public void UpdateMoveTimer()
        {
            _moveTimer -= Time.deltaTime;

            if (_moveTimer <= 0)
            {
                _characterAnimator.StopMovement();
                _navMeshAgent.ResetPath(); // Сброс пути при истечении таймера
                _destinationSet = false;
                _isMoving = false;
                _characterAnimator.SetMovement(0, 0, _isMoving);
                Task.current.Succeed(); // Таймер истек
            }
        }
    }
}
