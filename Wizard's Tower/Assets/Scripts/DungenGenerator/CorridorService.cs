using System.Collections.Generic;
using Factory;
using UnityEngine;

namespace DungenGenerator
{
    public class CorridorService
    {
        private RandomWalkService _randomWalkService;
        private DungeonFactory _dungeonFactory;

        public CorridorService(RandomWalkService randomWalkService, DungeonFactory dungeonFactory)
        {
            _dungeonFactory = dungeonFactory;
            _randomWalkService = randomWalkService;
        }

        public HashSet<Vector2Int> ConnectAllRoomsInOrder(List<Room> rooms, GameObject parent)
        {
            HashSet<Vector2Int> allCorridorPositions = new HashSet<Vector2Int>();

            if (rooms.Count > 1)
            {
                Room currentRoom = rooms[0];
                Vector2Int start = currentRoom.GetCenter();
                currentRoom.EntryPoint = start;

                for (int i = 1; i < rooms.Count; i++)
                {
                    Room nextRoom = rooms[i];

                    if (nextRoom.EntryPoint != Vector2Int.zero)
                    {
                        continue;
                    }

                    Vector2Int end = nextRoom.GetCenter();

                    HashSet<Vector2Int> corridor = _randomWalkService.GeneratePathBetweenPoints(start, end);

                    allCorridorPositions.UnionWith(corridor);

                    SpawnFloor(corridor, parent);

                    currentRoom.ExitPoints = end;
                    nextRoom.EntryPoint = end;

                    start = end;
                    currentRoom = nextRoom;
                }
            }

            return allCorridorPositions;
        }

        private void SpawnFloor(HashSet<Vector2Int> corridor, GameObject parent)
        {
            foreach (var pos in corridor)
            {
                Vector3 floorPosition = new Vector3(pos.x, 0f, pos.y);
                _dungeonFactory.SpawnFloor(floorPosition, parent.transform);
            }
        }
    }
}
