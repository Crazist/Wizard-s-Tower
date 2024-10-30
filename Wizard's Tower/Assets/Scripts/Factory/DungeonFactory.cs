using System.Collections.Generic;
using UnityEngine;
using DungenGenerator;
using Enemy;
using Player;
using Zenject;

namespace Factory
{
    public class DungeonFactory
    {
        private const string PlayerResourcePath = "Player";
        private const string SpawnConfigPath = "Configs/SpawnConfig";
        
        private SpawnConfig _spawnConfig;
       
        public PlayerController Player { get; set; }

        public DungeonFactory()
        {
            _spawnConfig = Resources.Load<SpawnConfig>(SpawnConfigPath);
        }

        public void SpawnPlayer(Vector3 pos)
        {
            var player = Resources.Load<PlayerController>(PlayerResourcePath);
            Player = Object.Instantiate(player, pos, Quaternion.identity);
        }
        public EnemyAIBase SpawnEnemy(EnemyAIBase gameObject) => 
            Object.Instantiate(gameObject);

        public void SpawnFloor(Vector3 position, Transform parent)
        {
            GameObject prefabToSpawn = GetPrefabByChance(_spawnConfig.FloorPrefabs);

            if (prefabToSpawn != null)
            {
                Object.Instantiate(prefabToSpawn, position, Quaternion.identity, parent);
            }
        }

        public GameObject SpawnWall(Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject prefabToSpawn = GetPrefabByChance(_spawnConfig.WallPrefabs);

            if (prefabToSpawn != null)
            {
                return Object.Instantiate(prefabToSpawn, position, rotation, parent);
            }

            return null;
        }

        public GameObject GetPrefabByChance(List<SpawnableItem> prefabs)
        {
            float randomValue = Random.value;
            float cumulativeChance = 0f;

            foreach (var spawnablePrefab in prefabs)
            {
                cumulativeChance += spawnablePrefab.SpawnChance;

                if (randomValue <= cumulativeChance)
                {
                    return spawnablePrefab.Prefab;
                }
            }

            return null;
        }

        public GameObject GetPrefabByChance(List<SpawnablePrefab> prefabs)
        {
            float randomValue = Random.value;
            float cumulativeChance = 0f;

            foreach (var spawnablePrefab in prefabs)
            {
                cumulativeChance += spawnablePrefab.Chance;

                if (randomValue <= cumulativeChance)
                {
                    return spawnablePrefab.Prefab;
                }
            }

            return null;
        }

        public GameObject InstantiateItem(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent) => 
            Object.Instantiate(prefab, position, rotation, parent);
    }
}