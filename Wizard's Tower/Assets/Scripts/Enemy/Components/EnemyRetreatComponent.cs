using Factory;
using Panda;
using UnityEngine;

namespace Enemy.Components
{
    public class EnemyRetreatComponent : MonoBehaviour
    {
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private RotationComponent _rotationComponent;
        [SerializeField] private float _retreatDistance = 3f;
        [SerializeField] private float _retreatSpeed = 2f;

        private DungeonFactory _dungeonFactory;
        private Vector3 _retreatPosition;

        public void Init(DungeonFactory dungeonFactory) =>
            _dungeonFactory = dungeonFactory;

        [Task]
        public void CalculateRetreatPosition()
        {
            if (_dungeonFactory.Player == null)
            {
                Task.current.Fail();
                return;
            }

            var position = _dungeonFactory.Player.transform.position;

            Vector3 retreatPosition =
                transform.position - (position - transform.position).normalized * _retreatDistance;
            Task.current.Succeed();
        }

        [Task]
        public void Retreat()
        {
            if (_dungeonFactory.Player == null)
            {
                Task.current.Fail();
                return;
            }

            var position = _dungeonFactory.Player.transform.position;

            _rotationComponent.RotateTowards(position);

            transform.position = Vector3.MoveTowards(
                transform.position,
                _retreatPosition,
                _retreatSpeed * Time.deltaTime
            );

            _animator.PlayRetreat();

            if (Vector3.Distance(transform.position, _retreatPosition) <= 0.1f)
            {
                Task.current.Succeed();
            }
        }
    }
}