using System.Collections.Generic;
using Factory;
using UnityEngine;
using Zenject;

namespace DungenGenerator
{
    public class WallService
    {
        private readonly Vector2Int[] _directions =
            { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        private ItemSpawner _itemSpawner;
        private DungeonFactory _dungeonFactory;

        [Inject]
        public void Construct(ItemSpawner itemSpawner, DungeonFactory dungeonFactory)
        {
            _dungeonFactory = dungeonFactory;
            _itemSpawner = itemSpawner;
        }

        public void GenerateWalls(List<Room> rooms, HashSet<Vector2Int> corridorPositions, GameObject parent,
            SpawnableItemsHolder itemsHolder)
        {
            HashSet<Vector2Int> floorPositions = GetAllFloorPositions(rooms);
            foreach (var corridorPos in corridorPositions)
            {
                floorPositions.Add(corridorPos);
            }

            foreach (var room in rooms)
            {
                AddWallPositionsToRoom(room, floorPositions);
            }

            foreach (var pos in floorPositions)
            {
                foreach (var dir in _directions)
                {
                    Vector2Int neighborPos = pos + dir;

                    if (!floorPositions.Contains(neighborPos))
                    {
                        Vector3 wallPosition = new Vector3(pos.x + dir.x * 0.5f, 0, pos.y + dir.y * 0.5f);
                        Quaternion wallRotation = GetWallRotation(dir);

                        GameObject wall = _dungeonFactory.SpawnWall(wallPosition, wallRotation, parent.transform);

                        _itemSpawner.SpawnTorch(wall.transform, wallPosition, wallRotation, itemsHolder);
                    }
                }
            }
        }

        private void AddWallPositionsToRoom(Room room, HashSet<Vector2Int> floorPositions)
        {
            foreach (var pos in room.RoomPositions)
            {
                foreach (var dir in _directions)
                {
                    Vector2Int neighborPos = pos + dir;

                    if (!floorPositions.Contains(neighborPos))
                    {
                        room.WallPositions.Add(neighborPos);
                    }
                }
            }
        }

        private HashSet<Vector2Int> GetAllFloorPositions(List<Room> rooms)
        {
            HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

            foreach (var room in rooms)
            {
                floorPositions.UnionWith(room.RoomPositions);
            }

            return floorPositions;
        }

        private Quaternion GetWallRotation(Vector2Int dir)
        {
            if (dir == Vector2Int.up) return Quaternion.Euler(0, 180, 0);
            if (dir == Vector2Int.down) return Quaternion.Euler(0, 0, 0);
            if (dir == Vector2Int.left) return Quaternion.Euler(0, 90, 0);

            return Quaternion.Euler(0, -90, 0);
        }
    }
}