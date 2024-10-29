using Factory;
using Panda;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Components
{
    public class EnemyRetreatComponent : MonoBehaviour
    {
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private RotationComponent _rotationComponent;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _minRetreatDistance = 2f;
        [SerializeField] private float _maxRetreatDistance = 5f;
        [SerializeField] private float _retreatSpeed = 2f;

        private DungeonFactory _dungeonFactory;
        private Vector3 _retreatPosition;
        
        public void Init(DungeonFactory dungeonFactory) => 
            _dungeonFactory = dungeonFactory;

        [Task]
        public void CalculateRetreatPosition()
        {
            if (_dungeonFactory.Player == null || _navMeshAgent == null)
            {
                Task.current.Fail();
                return;
            }

            var playerPosition = _dungeonFactory.Player.transform.position;
            float randomRetreatDistance = Random.Range(_minRetreatDistance, _maxRetreatDistance);

            Vector3 retreatDirection = (transform.position - playerPosition).normalized;
            Vector3 potentialRetreatPosition = transform.position + retreatDirection * randomRetreatDistance;

            if (NavMesh.SamplePosition(potentialRetreatPosition, out NavMeshHit hit, _maxRetreatDistance, NavMesh.AllAreas))
            {
                _retreatPosition = hit.position;
                Task.current.Succeed();
            }
            else
            {
                Task.current.Fail();
            }
        }

        [Task]
        public void Retreat()
        {
            if (_dungeonFactory.Player == null || _navMeshAgent == null)
            {
                Task.current.Fail();
                return;
            }

            var playerPosition = _dungeonFactory.Player.transform.position;
            _rotationComponent.RotateTowards(playerPosition);

           if (_navMeshAgent.isOnNavMesh)
            {
                _navMeshAgent.SetDestination(_retreatPosition);
                _animator.PlayRetreat();

                if (Vector3.Distance(transform.position, _retreatPosition) <= 0.1f)
                {
                    Task.current.Succeed();
                }
            }
            else
            {
                Task.current.Fail();
            }
        }
    }
}
