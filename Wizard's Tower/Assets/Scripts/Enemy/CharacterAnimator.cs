using UnityEngine;

namespace Enemy
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private const string _dashAnimationName = "DashForward";
        private const string _retreatAnimationName = "Retreat";
        private const string _attackAnimationName = "Attack";

        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        public void SetMovement(float horizontal, float vertical, bool isMoving)
        {
            if (_animator == null) return;

            SetHorizontal(horizontal);
            SetVertical(vertical);
            SetIsMoving(isMoving);
        }

        public void StopMovement()
        {
            if (_animator == null) return;

            SetHorizontal(0f);
            SetVertical(0f);
            SetIsMoving(false);
        }

        public void SetIsMoving(bool isMoving) => 
            _animator.SetBool(IsMoving, isMoving);

        public void PlayDash() => 
            _animator.Play(_dashAnimationName);

        public void PlayRetreat() => 
            _animator.Play(_retreatAnimationName);

        public void PlayAttack() => 
            _animator.Play(_attackAnimationName);

        private void SetHorizontal(float value) => 
            _animator.SetFloat(Horizontal, value);

        private void SetVertical(float value) => 
            _animator.SetFloat(Vertical, value);
    }
}