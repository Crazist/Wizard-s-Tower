using System;
using Pool;
using UnityEngine;

namespace VFX
{
    public class VFXObject : MonoBehaviour, IPoolable<VFXObject>
    {
        private ParticleSystem _particleSystem;
        private Action<VFXObject> _onReturnToPool;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public void Initialize(Action<VFXObject> onReturnToPool)
        {
            _onReturnToPool = onReturnToPool;
            var mainModule = _particleSystem.main;
            mainModule.stopAction = ParticleSystemStopAction.Callback;
        }

        public void Play(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
            gameObject.SetActive(true);
            _particleSystem.Play();
        }

        private void OnParticleSystemStopped()
        {
            _onReturnToPool?.Invoke(this);
        }

        public void OnReturnToPool()
        {
            _particleSystem.Stop();
            gameObject.SetActive(false);
        }
    }
}