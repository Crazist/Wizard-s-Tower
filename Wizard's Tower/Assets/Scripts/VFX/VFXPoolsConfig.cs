using Pool;
using UnityEngine;

namespace VFX
{
    [CreateAssetMenu(fileName = "VFXPoolsConfig", menuName = "Configs/VFXPoolsConfig")]
    public class VFXPoolsConfig : ScriptableObject
    {
        [SerializeField] private VFXObject _muzzleVFX;
        [SerializeField] private VFXObject _hitVFX;
        [SerializeField] private VFXObject _iceSpikesVFX;
        [SerializeField] private VFXObject _shieldVFX;
        [SerializeField] private int _initialPoolSize = 5;

        private ObjectPool<VFXObject> _fireballPool;
        private ObjectPool<VFXObject> _iceSpikesPool;
        private ObjectPool<VFXObject> _shieldPool;
        private ObjectPool<VFXObject> _hitPool;

        public ObjectPool<VFXObject> GetFireballPool()
        {
            if (_fireballPool == null)
            {
                _fireballPool = new ObjectPool<VFXObject>(_muzzleVFX, _initialPoolSize);
            }

            return _fireballPool;
        }

        public ObjectPool<VFXObject> GetIceSpikesPool()
        {
            if (_iceSpikesPool == null)
            {
                _iceSpikesPool = new ObjectPool<VFXObject>(_iceSpikesVFX, _initialPoolSize);
            }

            return _iceSpikesPool;
        }

        public ObjectPool<VFXObject> GetShieldPool()
        {
            if (_shieldPool == null)
            {
                _shieldPool = new ObjectPool<VFXObject>(_shieldVFX, _initialPoolSize);
            }

            return _shieldPool;
        }

        public ObjectPool<VFXObject> GetHitPool()
        {
            if (_hitPool == null)
            {
                _hitPool = new ObjectPool<VFXObject>(_hitVFX, _initialPoolSize);
            }

            return _hitPool;
        }
    }
}