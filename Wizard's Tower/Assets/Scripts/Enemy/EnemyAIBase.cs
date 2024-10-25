using System;
using Panda;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using DungenGenerator;
using System.Collections;

namespace Enemy
{
    public class EnemyAIBase : MonoBehaviour
    {
        [SerializeField] protected NavMeshAgent _navMeshAgent;
        
        protected Room CurrentRoom;
        protected MovementService MovementService;

        [Inject]
        private void Construct(MovementService movementService)
        {
            MovementService = movementService;
            WaitForSpawn();
        }

        public virtual void SetRoom(Room room) => 
            CurrentRoom = room;

        private void WaitForSpawn()
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
        public virtual void MoveToRandomPosition()
        {
            if (MovementService != null && _navMeshAgent != null && _navMeshAgent.enabled && CurrentRoom != null)
            {
                MovementService.MoveToRandomPosition(CurrentRoom, _navMeshAgent, 10f);
                Task.current.Succeed();
            }
            else
            {
                Task.current.Fail();
            }
        }
    }
}