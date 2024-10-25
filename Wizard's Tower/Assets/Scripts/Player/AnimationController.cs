using Factory;
using UnityEngine;
using Zenject;

namespace Player
{
    public class AnimationController : MonoBehaviour
    {
        private const string Speed = "Speed";
        private const string Turn = "Turn";
        private const string CastSpellTrigger = "CastSpell";

        [SerializeField] private Animator _animator;
        private UIFactory _uiFactory;
        private bool _canPlayMovementAnimation = true;

        [Inject]
        public void Construct(UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void UpdateMovementAnimation(float speed, float turn)
        {
            if (!_canPlayMovementAnimation) return;

            // Обновление скорости и направления поворота
            _animator.SetFloat(Speed, speed);
            _animator.SetFloat(Turn, turn);
        }

        public void TriggerCastSpell() => 
            _animator.SetTrigger(CastSpellTrigger);

        public void StopMovementAnimation()
        {
            _canPlayMovementAnimation = false;
            _animator.SetFloat(Speed, 0f);
            _animator.SetFloat(Turn, 0f);
        }

        public void PlayMovementAnimation()
        {
            _canPlayMovementAnimation = true;
        }
    }
}