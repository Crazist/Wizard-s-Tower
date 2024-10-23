using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemySpawnConfig", menuName = "Configs/EnemySpawnConfig")]
    public class EnemySpawnConfig : ScriptableObject
    {
        public List<EnemySpawnCondition> EnemySpawnConditions;
      
        public int MaxEnemiesCloseRooms;
        public int MinEnemiesCloseRooms;
        public int MaxEnemiesMediumRooms;
        public int MinEnemiesMediumRooms;
        public int MaxEnemiesFarRooms;
        public int MinEnemiesFarRooms;
    }
}