using System.Collections.Generic;
using Core.Factories.Interface;
using Core.Factories.Pools;
using DI;
using UnityEngine;

namespace Core.Factories
{
    public abstract class ObjectFactory<T> : MonoBehaviour, IPreInitializable, IFactory<T> where T : MonoBehaviour
    {
        [field: SerializeField] public T ObjPrefab { get; private set; }

        [field: SerializeField] public Transform ParentTr { get; private set; }

        public IPool<T> Pool { get; set; }

        public abstract void PreInitialize();


        public virtual T CreateObj()
        {
            return Pool.Get() ?? Instantiate(ObjPrefab, ParentTr);
        }

        public void DestroyObjs(List<T> emptyObjs)
        {
            foreach (var obj in emptyObjs) DestroyObj(obj);
        }

        public virtual void DestroyObj(T emptyObj)
        {
            Pool.Return(emptyObj);
        }
    }
}