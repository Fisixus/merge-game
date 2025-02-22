using System.Collections.Generic;
using Core.Factories.Pools;
using UnityEngine;

namespace Core.Factories.Interface
{
    public interface IFactory<T>
    {
        T CreateObj();

        void DestroyObjs(List<T> emptyItems);

        void DestroyObj(T emptyItem);

        T ObjPrefab { get; }

        Transform ParentTr { get; }

        IPool<T> Pool { get; }
    }
}