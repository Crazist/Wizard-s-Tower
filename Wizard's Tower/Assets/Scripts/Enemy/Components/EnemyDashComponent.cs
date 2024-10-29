using Factory;
using Panda;
using UnityEngine;

namespace Enemy.Components
{
    public class EnemyDashComponent : MonoBehaviour
    {
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private RotationComponent _rotationComponent;
        [SerializeField] private float _dashCooldown = 3f;

        private DungeonFactory _dungeonFactory;
        private float _lastDashTime;

        public void Init(DungeonFactory dungeonFactory) => 
            _dungeonFactory = dungeonFactory;

        [Task]
        public void TryDash()
        {
            if (_dungeonFactory.Player == null || Time.time - _lastDashTime < _dashCooldown)
            {
                Task.current.Fail();
                return;
            }

            _rotationComponent.RotateTowards(_dungeonFactory.Player.transform.position);
            _animator.PlayDash();
            _lastDashTime = Time.time;
            Task.current.Succeed();
        }
    }
}