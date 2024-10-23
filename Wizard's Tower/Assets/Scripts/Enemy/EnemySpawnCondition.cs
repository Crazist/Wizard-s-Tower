using UnityEngine;

namespace Enemy
{
    [System.Serializable]
    public class EnemySpawnCondition
    {
        public GameObject EnemyPrefab;
        public SpawnDistanceType SpawnDistance;
        public float SpawnChance;
    }
}