using System.Collections.Generic;
using UnityEngine;

namespace DungenGenerator
{
    [CreateAssetMenu(fileName = "SpawnableItemsHolder", menuName = "DungenGenerator/SpawnableItemsHolder")]
    public class SpawnableItemsHolder : ScriptableObject
    {
        public List<SpawnableItem> Items;
        public List<SpawnableItem> NearWallPrefabs;
        public List<SpawnableItem> AwayFromWallPrefabs;
    }
}