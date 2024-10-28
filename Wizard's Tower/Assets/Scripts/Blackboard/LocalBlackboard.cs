using System.Collections.Generic;
using UnityEngine;

namespace Blackboard
{
    public class LocalBlackboard : MonoBehaviour
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();

        public void Set<T>(string key, T value)
        {
            _data[key] = value;
        }

        public T Get<T>(string key)
        {
            if (_data.TryGetValue(key, out var value))
            {
                return (T)value;
            }

            return default;
        }

        public bool HasKey(string key)
        {
            return _data.ContainsKey(key);
        }

        public void RemoveKey(string key)
        {
            if (_data.ContainsKey(key))
            {
                _data.Remove(key);
            }
        }
    }
}