using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Sequence.Logic
{
    [Serializable]
    public class SerializableDictionary
    {
        [SerializeField] private List<CombatActionEntry> _entries = new List<CombatActionEntry>();

        public List<CombatActionEntry> Entries => _entries;

        public Dictionary<string, List<ICombatAction>> ToDictionary()
        {
            var dict = new Dictionary<string, List<ICombatAction>>();
            foreach (var entry in _entries)
            {
                var actionList = new List<ICombatAction>();
                foreach (var action in entry.Actions)
                {
                    // ReSharper disable once SuspiciousTypeConversion.Global
                    if (action is ICombatAction combatAction)
                    {
                        actionList.Add(combatAction);
                    }
                }
                dict[entry.Key] = actionList;
            }
            return dict;
        }
    }
}