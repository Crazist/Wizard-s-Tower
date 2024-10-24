using Panda;
using UnityEngine;
using UnityEngine.AI;
using DungenGenerator;

namespace Enemy
{
    public class KnightAIController : EnemyAIBase
    {
        [SerializeField] private float _knightWanderRadius = 15f;

        [Task]
        public override void MoveToRandomPosition()
        {
            if (MovementService != null && _navMeshAgent != null && _navMeshAgent.enabled && CurrentRoom != null)
            {
                MovementService.MoveToRandomPosition(CurrentRoom, _navMeshAgent, _knightWanderRadius);
                Task.current.Succeed();
            }
            else
            {
                Task.current.Fail();
            }
        }
    }
}