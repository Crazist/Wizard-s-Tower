using Blackboard;
using DungenGenerator;
using Panda;
using UnityEngine;

namespace Enemy.Components
{
    public class RandomPositionComponent : MonoBehaviour
    {
        [SerializeField] private LocalBlackboard _blackboard;
        private Room _currentRoom;
        private MovementService _movementService;

        public void Init(MovementService movementService, Room currentRoom)
        {
            _movementService = movementService;
            _currentRoom = currentRoom;
        }

        [Task]
        public void FindRandomPosition()
        {
            if (_currentRoom != null && _movementService != null)
            {
                Vector3 randomPosition = _movementService.GetRandomPointInRoom(_currentRoom);
                
                _blackboard.Set("TargetPosition", randomPosition);
                Task.current.Succeed();
                return;
            }

            Task.current.Fail();
        }
    }
}