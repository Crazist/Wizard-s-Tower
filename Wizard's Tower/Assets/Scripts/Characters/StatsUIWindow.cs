using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Characters
{
    public class StatsUIWindow : MonoBehaviour
    {
        [SerializeField] private RectTransform HealthBarParent;  // Родитель полоски здоровья
        [SerializeField] private RectTransform StaminaBarParent; // Родитель полоски стамины
        [SerializeField] private Image HealthFill;               // Заливка здоровья
        [SerializeField] private Image StaminaFill;              // Заливка стамины
        [SerializeField] private float MaxGameHealth = 1000f;    // Максимальное значение здоровья в игре

        private CharacterStats _playerStats;

        [Inject]
        private void Construct(StatsService statsService)
        {
            _playerStats = statsService.PlayerStats;

            // Подписка на события изменения здоровья и стамины
            _playerStats.OnHealthChanged += UpdateHealthBar;
            _playerStats.OnStaminaChanged += UpdateStaminaBar;

            // Инициализация начальных значений
            UpdateHealthBar();
            UpdateStaminaBar();
        }

        private void OnDestroy()
        {
            // Отписка от событий
            if (_playerStats != null)
            {
                _playerStats.OnHealthChanged -= UpdateHealthBar;
                _playerStats.OnStaminaChanged -= UpdateStaminaBar;
            }
        }

        private void UpdateHealthBar()
        {
            // Расчет текущей ширины родительского объекта
            float parentWidthPercent = _playerStats.MaxHealth / MaxGameHealth;
            HealthBarParent.anchorMax = new Vector2(parentWidthPercent, HealthBarParent.anchorMax.y);

            // Обновление заливки полоски в процентах от текущего здоровья
            HealthFill.fillAmount = _playerStats.CurrentHealth / _playerStats.MaxHealth;
        }

        private void UpdateStaminaBar()
        {
            // Расчет текущей ширины родительского объекта
            float parentWidthPercent = _playerStats.MaxStamina / MaxGameHealth;
            StaminaBarParent.anchorMax = new Vector2(parentWidthPercent, StaminaBarParent.anchorMax.y);

            // Обновление заливки полоски в процентах от текущей стамины
            StaminaFill.fillAmount = _playerStats.CurrentStamina / _playerStats.MaxStamina;
        }
    }
}
