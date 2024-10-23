using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Skills
{
    public class OrbUI : MonoBehaviour
    {
        [SerializeField] private List<Orb> _orbsSprites;
        
        [SerializeField] private Button _orbBtn;
        [SerializeField] private Image _orbImg;
        
        [SerializeField] public int _queuePosition;
        
        private SkillService _skillService;
        
        private int _currentSprite = 0;

        [Inject]
        private void Construct(SkillService skillService) => 
            _skillService = skillService;

        private void Start()
        {
            _orbBtn.onClick.AddListener(OrbClick);

            _orbImg.sprite = _orbsSprites[_currentSprite].Sprite;
            _skillService.AddOrb(_orbsSprites[_currentSprite], _queuePosition);
        }

        private void OrbClick()
        {
            _currentSprite = _currentSprite < (_orbsSprites.Count - 1) ? ++_currentSprite :  _currentSprite = 0;
       
            _orbImg.sprite = _orbsSprites[_currentSprite].Sprite;

            _skillService.OnOrbChange.Invoke(_orbsSprites[_currentSprite], _queuePosition);
        }
    }
}
