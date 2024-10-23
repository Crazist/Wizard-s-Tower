using System.Collections.Generic;
using UnityEngine;

namespace DungenGenerator
{
    [CreateAssetMenu(fileName = "SpawnConfig", menuName = "Configs/SpawnConfig")]
    public class SpawnConfig : ScriptableObject
    {
        public List<SpawnablePrefab> FloorPrefabs; 
        public List<SpawnablePrefab> WallPrefabs; 
    }
}