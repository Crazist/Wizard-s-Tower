using UnityEngine;
using Enemy.Components;

namespace Enemy
{
    public class KnightAIController : EnemyAIBase
    {
        [SerializeField] private MovementComponent _movementComponent;
        [SerializeField] private RotationComponent _rotationComponent;
        [SerializeField] private TimerComponent _timerComponent;
        [SerializeField] private RandomPositionComponent _randomPositionComponent;

        private void Start()
        {
            _randomPositionComponent.Init(MovementService, CurrentRoom);
        }
    }
}