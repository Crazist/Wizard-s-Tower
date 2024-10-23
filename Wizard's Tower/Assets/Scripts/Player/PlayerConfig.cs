using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Movement Settings")]
        [SerializeField] private float maxMoveSpeed = 5f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Cast Settings")]
        [SerializeField] private float castAnimationDuration = 2f;

        public float MaxMoveSpeed => maxMoveSpeed;
        public float Gravity => gravity;
        public float CastAnimationDuration => castAnimationDuration;
    }
}