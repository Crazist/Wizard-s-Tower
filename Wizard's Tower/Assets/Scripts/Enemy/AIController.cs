using Panda;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using System.Collections;

namespace Enemy
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _wanderRadius = 10f;
        private MovementService _movementService;

        [Inject]
        private void Construct(MovementService movementService) => 
            _movementService = movementService;

        private void Start()
        {
            if (_navMeshAgent != null)
            {
                _navMeshAgent.enabled = false;
                StartCoroutine(EnableNavMeshAgentAfterDelay(1f));
            }
        }

        private IEnumerator EnableNavMeshAgentAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (_navMeshAgent != null)
                _navMeshAgent.enabled = true;
        }

        [Task]
        public void MoveToRandomPosition()
        {
            if (_movementService != null && _navMeshAgent != null && _navMeshAgent.enabled)
            {
                _movementService.MoveToRandomPosition(transform.position, _navMeshAgent, _wanderRadius);
                Task.current.Succeed();
            }
            else
            {
                Task.current.Fail();
            }
        }
    }
}