using UnityEngine;
using UnityEngine.AI;
using DungenGenerator;
using System.Collections.Generic;

namespace Enemy
{
    public class MovementService
    {
        public void MoveToRandomPosition(Room room, NavMeshAgent agent, float radius)
        {
            if (room == null || agent == null) return;

            Vector3 randomPoint = GetRandomPointInRoom(room);

            if (randomPoint != Vector3.zero && NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }

        private Vector3 GetRandomPointInRoom(Room room)
        {
            List<Vector2Int> roomPositionsList = new List<Vector2Int>(room.RoomPositions);

            for (int i = 0; i < 10; i++) 
            {
                Vector2Int randomPos2D = roomPositionsList[Random.Range(0, roomPositionsList.Count)];
                Vector3 randomPoint = new Vector3(randomPos2D.x, 0, randomPos2D.y);

                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            Vector2Int center2D = room.GetCenter();
            return new Vector3(center2D.x, 0, center2D.y);
        }
    }
}