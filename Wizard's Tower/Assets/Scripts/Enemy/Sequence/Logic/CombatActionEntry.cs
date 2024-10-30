using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Sequence.Logic
{
    [Serializable]
    public class CombatActionEntry
    {
        public string Key;
        public List<MonoBehaviour> Actions;
    }
}