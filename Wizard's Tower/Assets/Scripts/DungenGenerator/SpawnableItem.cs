using UnityEngine;

namespace DungenGenerator
{
    [CreateAssetMenu(fileName = "SpawnableItem", menuName = "DungenGenerator/SpawnableItem")]
    public class SpawnableItem : ScriptableObject
    {
        public GameObject Prefab;
        
        [Range(0, 1)] public float SpawnChance;
        [Range(0, 20)] public float MinDistance;
    }
}