using Blackboard;
using Panda;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Components
{
    public class MovementComponent : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private CharacterAnimator _characterAnimator;
        [SerializeField] private LocalBlackboard _blackboard; 

        [Task]
        public void SetDestination()
        {
            if (!_blackboard.HasKey("TargetPosition"))
            {
                Task.current.Fail();
                return;
            }

            Vector3 targetPosition = _blackboard.Get<Vector3>("TargetPosition");
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(targetPosition);
            _characterAnimator.SetMovement(0, 1, true);
            Task.current.Succeed();
        }

        [Task]
        public void HasReachedDestination()
        {
            if (_navMeshAgent == null || _characterAnimator == null)
            {
                Task.current.Fail();
                return;
            }

            if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                _characterAnimator.StopMovement();
                Task.current.Succeed();
            }
        }

        [Task]
        public void Stop()
        {
            if (_navMeshAgent == null || _characterAnimator == null)
            {
                Task.current.Fail();
                return;
            }

            _navMeshAgent.isStopped = true;
            _navMeshAgent.ResetPath();
            _characterAnimator.StopMovement();
            Task.current.Succeed();
        }
        
    }
}
