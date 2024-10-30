using System;
using Factory;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Enemy.Sequence
{
    public class DashAction : MonoBehaviour, ICombatAction
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _dashDistance = 5f;
        [SerializeField] private float _dashSpeed = 15f;
        [SerializeField] private float _impactForce = 0.1f;
        [SerializeField] private float _dashCooldown = 3f;

        private DungeonFactory _dungeonFactory;
        private Vector3 _startPosition;
        private Vector3 _dashTargetPosition;
        private Vector3 _dashDirection;
        private Action _onComplete;
        private bool _isDashing;
        private float _lastDashTime;

        [Inject]
        public void Init(DungeonFactory dungeonFactory) =>
            _dungeonFactory = dungeonFactory;

        public void Execute(Action onComplete)
        {
            if (_isDashing || Time.time - _lastDashTime < _dashCooldown) return;

            _startPosition = transform.position;
            _dashDirection = (_dungeonFactory.Player.transform.position - _startPosition).normalized;
            _dashTargetPosition = _startPosition + _dashDirection * _dashDistance;
            _onComplete = onComplete;
            _isDashing = true;
            _lastDashTime = Time.time;

            _animator.PlayDash();

            // Добавляем кратковременный импульс через Rigidbody
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(_dashDirection * _impactForce, ForceMode.VelocityChange);

            // Отключаем навмеш и включаем для рывка
            _navMeshAgent.enabled = true;
            _navMeshAgent.speed = _dashSpeed;
            _navMeshAgent.SetDestination(_dashTargetPosition);
        }

        private void Update()
        {
            if (!_isDashing) return;

            // Проверка завершения рывка
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                EndDash();
            }
        }

        private void EndDash()
        {
            _isDashing = false;

            // Очищаем скорость Rigidbody и возвращаем в кинематическое состояние
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;

            // Сбрасываем настройки навмеша
            _navMeshAgent.ResetPath();
            _navMeshAgent.speed = 0;

            // Завершаем последовательность
            _onComplete?.Invoke();
        }

        public void Interrupt()
        {
            if (!_isDashing) return;

            _isDashing = false;

            // Прерываем рывок, останавливаем навмеш и Rigidbody
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            _navMeshAgent.ResetPath();
        }
    }
}
