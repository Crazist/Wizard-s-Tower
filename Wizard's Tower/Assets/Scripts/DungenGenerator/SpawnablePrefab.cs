using UnityEngine;

namespace DungenGenerator
{
    [System.Serializable]
    public class SpawnablePrefab
    {
        [Range(0f, 1f)]
        public float Chance;
        public GameObject Prefab;
    }
}