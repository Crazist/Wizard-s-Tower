using UnityEngine;
using Pool;

namespace Skills
{
    [CreateAssetMenu(fileName = "PoolsConfig", menuName = "Configs/PoolsConfig")]
    public class PoolsConfig : ScriptableObject
    {
        [SerializeField] private Fireball _fireballPrefab;
        [SerializeField] private int _initialPoolSize = 10;

        private ObjectPool<Fireball> _fireballPool;

        public ObjectPool<Fireball> GetFireballPool()
        {
            if (_fireballPool == null)
            {
                _fireballPool = new ObjectPool<Fireball>(_fireballPrefab, _initialPoolSize);
            }
            return _fireballPool;
        }
    }
}