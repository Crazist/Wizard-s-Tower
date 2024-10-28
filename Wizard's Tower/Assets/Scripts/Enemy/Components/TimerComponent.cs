using Panda;
using UnityEngine;

namespace Enemy.Components
{
    public class TimerComponent : MonoBehaviour
    {
        private float _timer;
        private bool _isRunning;

        [Task]
        public void SetRandomTimer(float minTime, float maxTime)
        {
            _timer = Random.Range(minTime, maxTime);
            _isRunning = true;
            Task.current.Succeed();
        }

        [Task]
        public void UpdateTimer()
        {
            if (!_isRunning)
            {
                Task.current.Fail();
                return;
            }

            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _isRunning = false;
                Task.current.Succeed();
            }
        }
    }
}