using System;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    [Serializable]
    public class Skill
    {
        public SkillType Type;
        public List<OrbType> SkillsRules;
        public Sprite Sprite;
        public GameObject Prefab;
        public float Cooldown;
    }
}
