using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _skillPosition;
        [SerializeField] private AnimationController _animationController;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerConfig _playerConfig;
        public Transform SkillPosition => _skillPosition;
        public AnimationController AnimationController => _animationController;
        public PlayerConfig PlayerConfig => _playerConfig;

        private bool _isCasting;

        private void Update()
        {
            if (_isCasting)
            {
              //  _playerMovement.StopMovement();
               // _animationController.StopMovementAnimation();
            }
        }

        public void CastSpell(float duration)
        {
            if (_isCasting) return;

            _isCasting = true;
            _animationController.TriggerCastSpell();

            //_playerMovement.StopMovement();
            //_animationController.StopMovementAnimation();

            StartCoroutine(CastingCoroutine(duration));
        }

        private IEnumerator CastingCoroutine(float duration)
        {
            yield return new WaitForSeconds(duration);

            _isCasting = false;
            _playerMovement.ResumeMovement();
            _animationController.PlayMovementAnimation();
        }
    }
}