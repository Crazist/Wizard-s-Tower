using Blackboard;
using Factory;
using Panda;
using UnityEngine;

namespace Enemy.Components
{
    public class RotationComponent : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 112.5f;
        [SerializeField] private float _rotationTolerance = 1f;
        [SerializeField] private LocalBlackboard _blackboard;

        private DungeonFactory _dungeonFactory;
        private bool _isRotatingToPlayer = false;

        public void Init(DungeonFactory dungeonFactory) => 
            _dungeonFactory = dungeonFactory;

        private void Update()
        {
            if (_isRotatingToPlayer && _dungeonFactory.Player != null)
            {
                RotateTowards(_dungeonFactory.Player.transform.position);
            }
        }

        [Task]
        public void StartRotateToPlayer()
        {
            _isRotatingToPlayer = true;
            Task.current.Succeed();
        }

        [Task]
        public void StopRotateToPlayer()
        {
            _isRotatingToPlayer = false;
            Task.current.Succeed();
        }

        [Task]
        public void RotateTowardsTarget()
        {
            if (!_blackboard.HasKey("TargetPosition"))
            {
                Task.current.Fail();
                return;
            }

            Vector3 targetPosition = _blackboard.Get<Vector3>("TargetPosition");

            RotateTowards(targetPosition);
            Task.current.Succeed();
        }

        public bool RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;

            if (Vector3.Angle(transform.forward, direction) <= _rotationTolerance)
            {
                return true;
            }

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * _rotationSpeed
            );

            return false;
        }

        public void RotateInstantly(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
