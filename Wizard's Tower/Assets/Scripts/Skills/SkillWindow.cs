using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Skills
{
    public class SkillWindow : MonoBehaviour
    {
        [SerializeField] private Button _useSkillBtn;
        [SerializeField] private Image _skillImage;
        [SerializeField] private CooldownUI _cooldown;

        private SkillService _skillService;

        [Inject]
        private void Construct(SkillService skillService)
        {
            _skillService = skillService;
            _useSkillBtn.onClick.AddListener(OnUseSkill);
            skillService.OnSkillChange += OnSkillSwitch;

            _cooldown.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_skillService.CurrentSkill != null)
            {
                float cooldown = _skillService.GetSkillCooldown(_skillService.CurrentSkill);
                float maxCooldown = _skillService.CurrentSkill.Cooldown;

                if (cooldown > 0)
                {
                    _cooldown.gameObject.SetActive(true);
                    _cooldown.UpdateCooldown(cooldown, maxCooldown);
                }
                else
                {
                    _cooldown.gameObject.SetActive(false);
                }
            }
        }

        private void OnSkillSwitch(Skill skill)
        {
            if(!_skillImage.gameObject.activeInHierarchy) _skillImage.gameObject.SetActive(true);
            
            _skillImage.sprite = skill.Sprite;

            float currentCooldown = _skillService.GetSkillCooldown(skill);
            if (currentCooldown > 0)
            {
                _cooldown.gameObject.SetActive(true);
                _cooldown.UpdateCooldown(currentCooldown, skill.Cooldown);
            }
            else
            {
                _cooldown.gameObject.SetActive(false);
            }
        }

        private void OnUseSkill()
        {
            if (_skillService.IsSkillOnCooldown(_skillService.CurrentSkill)) return;

            _skillService.UseSkill();
            _cooldown.StartCooldown();
        }
    }
}