using UnityEngine;

namespace Enemy
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        public void SetMovement(float horizontal, float vertical, bool isMoving)
        {
            if (_animator == null) return;

            _animator.SetFloat(Horizontal, horizontal);
            _animator.SetFloat(Vertical, vertical);
            _animator.SetBool(IsMoving, isMoving);
        }

        public void StopMovement()
        {
            if (_animator == null) return;

            _animator.SetFloat(Horizontal, 0f);
            _animator.SetFloat(Vertical, 0f);
            _animator.SetBool(IsMoving, false);
        }
    }
}