using Factory;
using Panda;
using UnityEngine;

namespace Enemy.Components
{
    public class EnemyAttackComponent : MonoBehaviour
    {
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private float _attackRange = 2.0f;
        [SerializeField] private float _attackCooldown = 1.5f;

        private DungeonFactory _dungeonFactory;
        private float _lastAttackTime;

        public void Init(DungeonFactory dungeonFactory) => 
            _dungeonFactory = dungeonFactory;

        [Task]
        public void TryAttack()
        {
            if (_dungeonFactory.Player == null || Time.time - _lastAttackTime < _attackCooldown)
            {
                Task.current.Fail();
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, _dungeonFactory.Player.transform.position);

            if (distanceToPlayer <= _attackRange)
            {
                _animator.PlayAttack(); 
                _lastAttackTime = Time.time;
                Task.current.Succeed();
            }
            else
            {
                Task.current.Fail();
            }
        }
    }
}