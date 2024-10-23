using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Skills
{
    public class CooldownUI : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private TMP_Text _cooldownText;

        public void StartCooldown()
        {
            gameObject.SetActive(true);
            _fillImage.fillAmount = 1f;
        }

        public void UpdateCooldown(float currentCooldown, float maxCooldown)
        {
            float progress = Mathf.Clamp01(currentCooldown / maxCooldown);
            _fillImage.fillAmount = progress;
            _cooldownText.text = Mathf.Ceil(currentCooldown).ToString(CultureInfo.InvariantCulture);
        }
    }
}