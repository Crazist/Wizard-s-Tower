using UnityEngine;

namespace Enemy
{
    [System.Serializable]
    public class EnemySpawnCondition
    {
        public EnemyAIBase _enemyAIBase;
        public SpawnDistanceType SpawnDistance;
        public float SpawnChance;
    }
}