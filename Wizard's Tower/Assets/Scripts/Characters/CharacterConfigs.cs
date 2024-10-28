using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "CharacterConfigs", menuName = "Configs/CharacterConfigs")]
    public class CharacterConfigs : ScriptableObject
    {
    
        public List<CharacterBaseStatsConfig> CharacterBaseStatsConfig;

        public CharacterBaseStatsConfig GetCharacterStats(CharacterType type) => 
            CharacterBaseStatsConfig.Find(config => config.Type == type);
    }
}