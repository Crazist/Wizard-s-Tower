using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "SkillConfig", menuName = "Configs/SkillConfig")]
    public class SkillConfig : ScriptableObject
    {
        public List<Skill> SkillsRules;
    }
}