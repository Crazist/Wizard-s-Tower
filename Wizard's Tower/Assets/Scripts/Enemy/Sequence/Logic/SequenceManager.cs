using System.Collections.Generic;
using Panda;
using UnityEngine;

namespace Enemy.Sequence.Logic
{
    public class SequenceManager : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary _actionDictionary;
        private Dictionary<string, List<ICombatAction>> _sequences = new Dictionary<string, List<ICombatAction>>();

        private List<ICombatAction> _currentSequence;
        private bool _isRunning = false;
        private int _currentActionIndex;

        private void Awake() => 
            _sequences = _actionDictionary.ToDictionary();

        [Task]
        public void StartSequence(string sequenceNumber)
        {
            string key = sequenceNumber.ToString();

            if (_isRunning)
            {
                Debug.LogWarning("Another sequence is already running.");
                Task.current.Fail();
                return;
            }

            if (!_sequences.ContainsKey(key))
            {
                Debug.LogWarning($"Sequence with key '{key}' not found.");
                Task.current.Fail();
                return;
            }

            _currentSequence = _sequences[key];
            _currentActionIndex = 0;
            _isRunning = true;

            ExecuteNextAction();
            Task.current.Succeed();
        }

        [Task]
        public void IsSequenceRunning()
        {
            if (_isRunning)
            {
                Task.current.Succeed();
            }
            else
            {
                Task.current.Fail();
            }
        }

        private void ExecuteNextAction()
        {
            if (_currentActionIndex >= _currentSequence.Count)
            {
                _isRunning = false;
                return;
            }

            var currentAction = _currentSequence[_currentActionIndex];
            _currentActionIndex++;

            currentAction.Execute(() =>
            {
                if (_isRunning)
                {
                    if (_currentActionIndex >= _currentSequence.Count)
                    {
                        StopSequenceInternal();
                    }
                    else
                    {
                        ExecuteNextAction();
                    }
                }
            });
        }

        private void StopSequenceInternal() => 
            _isRunning = false;
    }
}
