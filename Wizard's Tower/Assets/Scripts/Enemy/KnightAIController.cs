using UnityEngine;
using Enemy.Components;
using Factory;
using Zenject;

namespace Enemy
{
    public class KnightAIController : EnemyAIBase
    {
        [SerializeField] private MovementComponent _movementComponent;
        [SerializeField] private RotationComponent _rotationComponent;
        [SerializeField] private TimerComponent _timerComponent;
        [SerializeField] private RandomPositionComponent _randomPositionComponent;
        [SerializeField] private EnemyDashComponent _enemyDashComponent;
        [SerializeField] private EnemyRetreatComponent _enemyRetreatComponent;
        [SerializeField] private EnemyAttackComponent _attackComponent;
        [SerializeField] private EnemyVisionComponent _enemyVisionComponent;
        
        private DungeonFactory _dungeonFactory;

        [Inject]
        public void Inject(DungeonFactory dungeonFactory) => 
            _dungeonFactory = dungeonFactory;

        private void Start()
        {
            _randomPositionComponent.Init(MovementService, CurrentRoom);
            _enemyDashComponent.Init(_dungeonFactory);
            _enemyRetreatComponent.Init(_dungeonFactory);
            _attackComponent.Init(_dungeonFactory);
            _enemyVisionComponent.Init(_dungeonFactory);
            _rotationComponent.Init(_dungeonFactory);
        }
    }
}