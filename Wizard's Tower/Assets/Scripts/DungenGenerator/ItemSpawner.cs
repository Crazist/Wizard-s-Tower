using System.Collections.Generic;
using UnityEngine;
using Factory;
using Zenject;

namespace DungenGenerator
{
    public class ItemSpawner
    {
        private DungeonFactory _factory;
        private List<GameObject> _spawnedItems = new List<GameObject>(); // Список спауненных объектов

        [Inject]
        private void Construct(DungeonFactory factory) => _factory = factory;

        public void SpawnTorch(Transform parent, Vector3 wallPosition, Quaternion wallRotation, SpawnableItemsHolder itemsHolder)
        {
            foreach (var item in itemsHolder.Items)
            {
                if (Random.value <= item.SpawnChance)
                {
                    Vector3 offset = wallRotation * new Vector3(0, 0.5f, 0.08f);
                    Vector3 torchPosition = wallPosition + offset;

                    if (IsPositionValid(torchPosition, item.MinDistance))
                    {
                        var spawnedTorch = _factory.InstantiateItem(item.Prefab, torchPosition, wallRotation, parent);
                        _spawnedItems.Add(spawnedTorch); // Добавляем объект в список

                        break;
                    }
                }
            }
        }

        public void SpawnItemsInRooms(List<Room> rooms, Transform parent, SpawnableItemsHolder itemsHolder)
        {
            foreach (var room in rooms)
            {
                SpawnNearWalls(room, parent, itemsHolder.NearWallPrefabs);
                SpawnAwayFromWalls(room, parent, itemsHolder.AwayFromWallPrefabs);
            }
        }

        public void Clear()
        {
            // Уничтожаем все спауненные объекты
            foreach (var item in _spawnedItems)
            {
                if (item != null) Object.Destroy(item);
            }

            _spawnedItems.Clear(); // Очищаем список
        }

        private void SpawnNearWalls(Room room, Transform parent, List<SpawnableItem> prefabs)
        {
            foreach (var wallPos in room.WallPositions)
            {
                Vector3 position = new Vector3(wallPos.x, 0, wallPos.y);

                Vector3 offset;
                Quaternion rotation;
                GetWallOffsetAndRotation(wallPos, room, out offset, out rotation);

                Vector3 spawnPosition = position + offset;

                GameObject prefabToSpawn = _factory.GetPrefabByChance(prefabs);

                if (prefabToSpawn != null && IsPositionValid(spawnPosition, 2f))
                {
                    var spawnedItem = _factory.InstantiateItem(prefabToSpawn, spawnPosition, rotation, parent);
                    _spawnedItems.Add(spawnedItem); // Добавляем объект в список
                }
            }
        }

        private void SpawnAwayFromWalls(Room room, Transform parent, List<SpawnableItem> prefabs)
        {
            List<Vector2Int> floorPositions = new List<Vector2Int>(room.RoomPositions);

            foreach (var pos in floorPositions)
            {
                if (IsNearWall(pos, room.WallPositions, minDistanceFromWall: 1)) continue;

                Vector3 position = new Vector3(pos.x + 0.5f, 0, pos.y + 0.5f);
                GameObject prefabToSpawn = _factory.GetPrefabByChance(prefabs);

                if (prefabToSpawn != null && IsPositionValid(position, 2f))
                {
                    var spawnedItem = _factory.InstantiateItem(prefabToSpawn, position, Quaternion.identity, parent);
                    _spawnedItems.Add(spawnedItem); // Добавляем объект в список
                }
            }
        }

        private void GetWallOffsetAndRotation(Vector2Int wallPos, Room room, out Vector3 offset, out Quaternion rotation)
        {
            offset = Vector3.zero;
            rotation = Quaternion.identity;

            if (room.RoomPositions.Contains(wallPos + Vector2Int.up))
            {
                offset = new Vector3(0, 0, 0.8f);
                rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (room.RoomPositions.Contains(wallPos + Vector2Int.down))
            {
                offset = new Vector3(0, 0, -0.8f);
                rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (room.RoomPositions.Contains(wallPos + Vector2Int.left))
            {
                offset = new Vector3(-0.8f, 0, 0);
                rotation = Quaternion.Euler(0, 90, 0);
            }
            else if (room.RoomPositions.Contains(wallPos + Vector2Int.right))
            {
                offset = new Vector3(0.8f, 0, 0);
                rotation = Quaternion.Euler(0, -90, 0);
            }
        }

        private bool IsNearWall(Vector2Int pos, HashSet<Vector2Int> wallPositions, int minDistanceFromWall)
        {
            Vector2Int[] directions = 
            {
                Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
                Vector2Int.up + Vector2Int.left, Vector2Int.up + Vector2Int.right,
                Vector2Int.down + Vector2Int.left, Vector2Int.down + Vector2Int.right
            };

            foreach (var dir in directions)
            {
                for (int i = 1; i <= minDistanceFromWall; i++)
                {
                    if (wallPositions.Contains(pos + dir * i))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsPositionValid(Vector3 position, float minDistance)
        {
            foreach (var spawnedItem in _spawnedItems)
            {
                if (spawnedItem == null) continue;

                if (Vector3.Distance(position, spawnedItem.transform.position) < minDistance)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
