using System.Collections.Generic;
using UnityEngine;

namespace Core.Factories.Pools
{
    public class ObjectPool<T> : IPool<T> where T : Component
    {
        private readonly Queue<T> _pool = new();
        private readonly T _prefab;
        private readonly Transform _parentTr;

        public ObjectPool()
        {
        }

        public ObjectPool(T prefab, Transform parentTr, int poolSize = 64)
        {
            _prefab = prefab;
            _parentTr = parentTr;

            // Preload the pool with inactive objects
            for (var i = 0; i < poolSize; i++)
            {
                var obj = Object.Instantiate(_prefab, _parentTr);
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        public T Get()
        {
            if (_pool.Count > 0)
            {
                var obj = _pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            // If the pool is empty, create a new instance
            var newObj = Object.Instantiate(_prefab, _parentTr);
            return newObj;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}