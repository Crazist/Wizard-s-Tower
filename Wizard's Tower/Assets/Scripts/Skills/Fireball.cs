using System;
using UnityEngine;
using Pool;

namespace Skills
{
    public class Fireball : MonoBehaviour, IPoolable<Fireball>
    {
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _lifetime = 2f;
        private float _currentLifetime;
        private bool _isActive;

        private Action<Fireball> _onReturnToPool;

        public event Action<Vector3> OnHit;

        public void Initialize(Action<Fireball> onReturnToPool)
        {
            _onReturnToPool = onReturnToPool;
        }

        public void Activate(Vector3 startPosition, Vector3 direction)
        {
            transform.position = startPosition;
            transform.forward = direction;
            _currentLifetime = _lifetime;
            _isActive = true;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (!_isActive) return;

            transform.Translate(Vector3.forward * (_speed * Time.deltaTime));

            _currentLifetime -= Time.deltaTime;
            if (_currentLifetime <= 0f)
            {
                Explode();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                OnHit?.Invoke(transform.position);
                Explode();
            }
        }

        private void Explode()
        {
            Debug.Log("Fireball Exploded!");

            _isActive = false;
            _onReturnToPool?.Invoke(this);
        }

        public void OnReturnToPool()
        {
            OnHit = null;
            gameObject.SetActive(false);
        }
    }
}