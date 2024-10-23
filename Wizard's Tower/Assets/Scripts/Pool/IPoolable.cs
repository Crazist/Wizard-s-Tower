using System;

namespace Pool
{
    public interface IPoolable<T>
    {
        void Initialize(Action<T> onReturnToPool);
        void OnReturnToPool();
    }
}