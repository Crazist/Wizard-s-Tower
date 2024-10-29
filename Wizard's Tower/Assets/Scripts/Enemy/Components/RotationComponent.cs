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

        public void Init(DungeonFactory dungeonFactory) => 
            _dungeonFactory = dungeonFactory;

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
        }
        [Task]
        public void RotateToPlayer()
        {
            Task.current.Succeed();

            RotateTowards(_dungeonFactory.Player.transform.position);
        }
        public void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;

            if (Vector3.Angle(transform.forward, direction) <= _rotationTolerance)
            {
                Task.current.Succeed();
                return;
            }

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * _rotationSpeed
            );
        }

        public void RotateInstantly(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}