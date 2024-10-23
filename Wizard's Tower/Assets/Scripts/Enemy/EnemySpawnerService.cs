using System.Collections.Generic;
using DungenGenerator;
using Factory;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class EnemySpawnerService
    {
        private readonly DungeonFactory _dungeonFactory;
        private readonly AssetProvider _assetProvider;

        [Inject]
        public EnemySpawnerService(DungeonFactory dungeonFactory, AssetProvider assetProvider)
        {
            _dungeonFactory = dungeonFactory;
            _assetProvider = assetProvider;
        }

        public void SpawnEnemiesInRooms(List<Room> allRooms)
        {
            // Исключаем первую комнату
            if (allRooms.Count > 1)
                allRooms.RemoveAt(0);
            else
                return;

            // Разделяем комнаты на ближние, средние и дальние
            var (nearRooms, mediumRooms, farRooms) = SplitRoomsIntoGroups(allRooms);

            // Спаун врагов начиная с дальних комнат
            SpawnEnemiesInGroup(farRooms, SpawnDistanceType.Far, _assetProvider.EnemySpawnConfig.MaxEnemiesFarRooms,
                _assetProvider.EnemySpawnConfig.MinEnemiesFarRooms);
            SpawnEnemiesInGroup(mediumRooms, SpawnDistanceType.Medium,
                _assetProvider.EnemySpawnConfig.MaxEnemiesMediumRooms,
                _assetProvider.EnemySpawnConfig.MinEnemiesMediumRooms);
            SpawnEnemiesInGroup(nearRooms, SpawnDistanceType.Close,
                _assetProvider.EnemySpawnConfig.MaxEnemiesCloseRooms,
                _assetProvider.EnemySpawnConfig.MinEnemiesCloseRooms);
        }

        private (List<Room> nearRooms, List<Room> mediumRooms, List<Room> farRooms) SplitRoomsIntoGroups(
            List<Room> allRooms)
        {
            int totalRooms = allRooms.Count;
            int roomsPerGroup = Mathf.CeilToInt(totalRooms / 3f);

            List<Room> nearRooms = allRooms.GetRange(0, Mathf.Min(roomsPerGroup, totalRooms));
            List<Room> mediumRooms =
                allRooms.GetRange(roomsPerGroup, Mathf.Min(roomsPerGroup, totalRooms - roomsPerGroup));
            List<Room> farRooms = allRooms.GetRange(roomsPerGroup * 2,
                Mathf.Min(roomsPerGroup, totalRooms - roomsPerGroup * 2));

            return (nearRooms, mediumRooms, farRooms);
        }

        private void SpawnEnemiesInGroup(List<Room> rooms, SpawnDistanceType distanceType, int maxEnemies,
            int minEnemies)
        {
            foreach (var room in rooms)
            {
                int enemiesToSpawn = Random.Range(minEnemies, maxEnemies + 1);

                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    foreach (var spawnCondition in _assetProvider.EnemySpawnConfig.EnemySpawnConditions)
                    {
                        // Проверяем, соответствует ли дистанция спауна
                        if (spawnCondition.SpawnDistance != distanceType)
                            continue;

                        float randomValue = Random.value;
                        if (randomValue <= spawnCondition.SpawnChance)
                        {
                            Vector3 spawnPosition = GetRandomPositionInRoom(room);
                            SpawnEnemy(spawnCondition.EnemyPrefab, spawnPosition, Quaternion.identity);
                        }
                    }
                }
            }
        }

        private void SpawnEnemy(GameObject enemyPrefab, Vector3 position, Quaternion rotation)
        {
            GameObject newEnemy = _dungeonFactory.SpawnEnemy(enemyPrefab);
            newEnemy.transform.position = position;
            newEnemy.transform.rotation = rotation;
        }

        private Vector3 GetRandomPositionInRoom(Room room)
        {
            // Находим случайную позицию в пределах комнаты
            List<Vector2Int> positions = new List<Vector2Int>(room.RoomPositions);
            Vector2Int randomPosition = positions[Random.Range(0, positions.Count)];
            return new Vector3(randomPosition.x, 0, randomPosition.y);
        }
    }
}