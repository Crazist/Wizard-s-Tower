using Blackboard;
using Factory;
using Panda;
using UnityEngine;

namespace Enemy.Components
{
    public class EnemyVisionComponent : MonoBehaviour
    {
        [SerializeField] private LocalBlackboard _blackboard;
        [SerializeField] private float _viewRadius = 6f;   
        [SerializeField] private float _triggerRadius = 3f;   
        [SerializeField] private float _viewAngle = 45f;
        [SerializeField] private LayerMask _targetMask;  
        [SerializeField] private LayerMask _obstacleMask; 
        [SerializeField] private float _eyeHeightOffset = 0.2f; 

        private DungeonFactory _dungeonFactory;

        public void Init(DungeonFactory dungeonFactory) =>
            _dungeonFactory = dungeonFactory;

        [Task]
        public void CheckPlayerVisibility()
        {
            if (_dungeonFactory == null || _dungeonFactory.Player == null)
            {
                Task.current.Fail();
                return;
            }

            Vector3 enemyEyePosition = transform.position + Vector3.up * _eyeHeightOffset;
            Vector3 playerPosition = _dungeonFactory.Player.transform.position + Vector3.up * _eyeHeightOffset;

            Vector3 directionToPlayer = (playerPosition - enemyEyePosition).normalized;
            float distanceToPlayer = Vector3.Distance(enemyEyePosition, playerPosition);

            if (distanceToPlayer <= _viewRadius && Vector3.Angle(transform.forward, directionToPlayer) <= _viewAngle / 2)
            {
                if (Physics.Raycast(enemyEyePosition, directionToPlayer, out RaycastHit hit, _viewRadius, _targetMask | _obstacleMask))
                {
                    if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & _targetMask) != 0)
                    {
                        _blackboard.Set("IsPlayerVisible", true);
                        Task.current.Succeed();
                        return;
                    }
                }
            }
            
            if (distanceToPlayer <= _triggerRadius)
            {
                _blackboard.Set("IsPlayerVisible", true);
                Task.current.Succeed();
                return;
            }
            
            _blackboard.Set("IsPlayerVisible", false);
            Task.current.Fail();
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 enemyEyePosition = transform.position + Vector3.up * _eyeHeightOffset;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _viewRadius);

            Vector3 leftBoundary = Quaternion.Euler(0, -_viewAngle / 2, 0) * transform.forward * _viewRadius;
            Vector3 rightBoundary = Quaternion.Euler(0, _viewAngle / 2, 0) * transform.forward * _viewRadius;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(enemyEyePosition, enemyEyePosition + leftBoundary);
            Gizmos.DrawLine(enemyEyePosition, enemyEyePosition + rightBoundary);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _triggerRadius);

            if (_blackboard != null && _blackboard.HasKey("IsPlayerVisible") && _blackboard.Get<bool>("IsPlayerVisible"))
            {
                Gizmos.color = Color.red;
                if (_dungeonFactory != null && _dungeonFactory.Player != null)
                {
                    Vector3 playerEyePosition = _dungeonFactory.Player.transform.position + Vector3.up * _eyeHeightOffset;
                    Gizmos.DrawLine(enemyEyePosition, playerEyePosition);
                }
            }
        }
    }
}
