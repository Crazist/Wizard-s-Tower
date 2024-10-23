using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private T _prefab;
        private Transform _parent;
        private Queue<T> _pool;

        public ObjectPool(T prefab, int initialSize, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            _pool = new Queue<T>();

            for (int i = 0; i < initialSize; i++)
            {
                CreateNewObject();
            }
        }

        private T CreateNewObject()
        {
            var obj = UnityEngine.Object.Instantiate(_prefab, _parent);
            obj.Initialize(ReturnToPool);
            obj.OnReturnToPool();
            _pool.Enqueue(obj);
            return obj;
        }

        public T Get()
        {
            if (_pool.Count > 0)
            {
                var obj = _pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            return CreateNewObject();
        }

        public void ReturnToPool(T obj)
        {
            obj.OnReturnToPool();
            _pool.Enqueue(obj);
        }
    }
}