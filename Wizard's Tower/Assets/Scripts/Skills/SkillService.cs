using System;
using System.Collections.Generic;
using System.Linq;
using Factory;
using VFX;
using Zenject;
using UnityEngine;

namespace Skills
{
    public class SkillService : ITickable
    {
        public Skill CurrentSkill { get; private set; }

        public Action<Orb, int> OnOrbChange;
        public Action<Skill> OnSkillChange;

        private List<Orb> _orbs = new() { null, null, null };
        private Dictionary<Skill, float> _skillCooldowns = new();
        
        private AssetProvider _assetProvider;
        private FireballSpawner _fireballSpawner;

        [Inject]
        private void Construct(AssetProvider assetProvider, DungeonFactory dungeonFactory, VFXService vfxService)
        {
            _fireballSpawner = new FireballSpawner(assetProvider.PoolsConfig, dungeonFactory, vfxService);
            _assetProvider = assetProvider;
            OnOrbChange += ChangeOrb;
        }

        public void Tick()
        {
            foreach (var skill in _skillCooldowns.Keys.ToList())
            {
                if (_skillCooldowns[skill] > 0)
                {
                    _skillCooldowns[skill] -= Time.deltaTime;
                }
            }
        }

        public void AddOrb(Orb orb, int queuePosition) => 
            _orbs[queuePosition] = orb;

        public void UseSkill()
        {
            if (CurrentSkill == null || IsSkillOnCooldown(CurrentSkill)) return;

            StartSkillCooldown(CurrentSkill);
            _fireballSpawner.Shoot();
        }

        public bool IsSkillOnCooldown(Skill skill) => 
            _skillCooldowns.TryGetValue(skill, out var cooldown) && cooldown > 0;

        private void ChangeOrb(Orb orb, int queuePosition)
        {
            _orbs[queuePosition] = orb;
            ChangeSkill();
        }

        private void ChangeSkill()
        {
            var skill = _assetProvider.SkillConfig.SkillsRules.FirstOrDefault(skills =>
                skills.SkillsRules.SequenceEqual(_orbs.Select(orb => orb.Type)));

            if (skill != null && skill != CurrentSkill)
            {
                CurrentSkill = skill;
                OnSkillChange?.Invoke(CurrentSkill);
            }
        }

        private void StartSkillCooldown(Skill skill)
        {
            if (skill != null)
            {
                _skillCooldowns[skill] = skill.Cooldown;
            }
        }

        public float GetSkillCooldown(Skill skill) => 
            _skillCooldowns.TryGetValue(skill, out var cooldown) ? cooldown : 0;
    }
}
