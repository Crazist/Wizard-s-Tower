using Blackboard;
using Panda;
using UnityEngine;

namespace Enemy.Components
{
    public class EnemyReactionComponent : MonoBehaviour
    {
        [SerializeField] private LocalBlackboard _blackboard;
        [SerializeField] private float _reactionTime = 5.0f;
        private bool _isPlayerVisible = false;
        private float _lastSeenTime = -Mathf.Infinity;

        [Task]
        public void UpdateVisionState()
        {
            if (!_blackboard.HasKey("IsPlayerVisible"))
            {
                Task.current.Fail();
                return;
            }

            bool isPlayerCurrentlyVisible = _blackboard.Get<bool>("IsPlayerVisible");

            if (isPlayerCurrentlyVisible && !_isPlayerVisible)
            {
                _lastSeenTime = Time.time;
            }

            _isPlayerVisible = isPlayerCurrentlyVisible;

            if (_isPlayerVisible || (Time.time - _lastSeenTime <= _reactionTime))
            {
                Task.current.Succeed();
            }
            else
            {
                Task.current.Fail();
            }
        }
    }
}