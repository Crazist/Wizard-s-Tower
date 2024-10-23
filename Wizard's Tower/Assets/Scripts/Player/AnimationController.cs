using Factory;
using UnityEngine;
using Zenject;

namespace Player
{
    public class AnimationController : MonoBehaviour
    {
        private const string Speed = "Speed";
        private const string CastSpellTrigger = "CastSpell";

        [SerializeField] private Animator _animator;
        public Animator Animator => _animator;

        private UIFactory _uiFactory;
        private bool _canPlayMovementAnimation = true;

        [Inject]
        public void Construct(UIFactory uiFactory) =>
            _uiFactory = uiFactory;

        private void Update()
        {
            if (_uiFactory.FixedJoystick == null) return;

            HandleMovementAnimation();
        }

        private void HandleMovementAnimation()
        {
            if (!_canPlayMovementAnimation)
            {
                _animator.SetFloat(Speed, 0f);
                return;
            }

            float horizontal = _uiFactory.FixedJoystick.Horizontal;
            float vertical = _uiFactory.FixedJoystick.Vertical;
            float speed = new Vector2(horizontal, vertical).magnitude;

            _animator.SetFloat(Speed, speed);
        }

        public void TriggerCastSpell() => 
            _animator.SetTrigger(CastSpellTrigger);

        public void StopMovementAnimation() =>
            _canPlayMovementAnimation = false;

        public void PlayMovementAnimation() =>
            _canPlayMovementAnimation = true;
    }
}