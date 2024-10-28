using System;
using UnityEngine;

namespace Characters
{
    public class CharacterStats
    {
        public float CurrentHealth { get; private set; }
        public float CurrentStamina { get; private set; }

        public float MaxHealth { get; private set; }
        public float MaxStamina { get; private set; }

        public event Action OnHealthChanged;
        public event Action OnStaminaChanged;

        public CharacterStats(CharacterBaseStatsConfig baseStats)
        {
            MaxHealth = baseStats.MaxHealth;
            MaxStamina = baseStats.MaxStamina;
            CurrentHealth = MaxHealth;
            CurrentStamina = MaxStamina;
        }

        public void ModifyHealth(float amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
            OnHealthChanged?.Invoke();
        }

        public void ModifyStamina(float amount)
        {
            CurrentStamina = Mathf.Clamp(CurrentStamina + amount, 0, MaxStamina);
            OnStaminaChanged?.Invoke();
        }
    }
}