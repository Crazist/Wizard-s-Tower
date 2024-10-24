using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class MovementService
    {
        public void MoveToRandomPosition(Vector3 center, NavMeshAgent agent, float radius)
        {
            Vector3 randomPoint = GetRandomPoint(center, radius);
            agent.SetDestination(randomPoint);
        }

        private Vector3 GetRandomPoint(Vector3 center, float radius)
        {
            Vector2 randomPoint = Random.insideUnitCircle * radius;
            Vector3 finalPoint = center + new Vector3(randomPoint.x, 0, randomPoint.y);

            if (NavMesh.SamplePosition(finalPoint, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return center;
        }
    }
}