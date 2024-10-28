using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "CharacterBaseStats", menuName = "Configs/CharacterBaseStats")]
    public class CharacterBaseStatsConfig : ScriptableObject
    {
        public CharacterType Type;
        public float MaxHealth;
        public float MaxStamina;
    }
}