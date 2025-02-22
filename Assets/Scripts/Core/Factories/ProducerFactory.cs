using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Core.Factories.Interface;
using Core.Factories.Pools;
using Core.GridPawns;
using Core.GridPawns.Enum;
using UnityEngine;

namespace Core.Factories
{
    public class ProducerFactory : ObjectFactory<Producer>, IProducerFactory
    {
    
        [field: SerializeField]
        [SerializedDictionary("Producer Type", "Producer Data")]
        public SerializedDictionary<ProducerType, ProducerDataSO> ProducerDataDict { get; private set; }
        
        private List<Producer> _allProducers;

        public override void PreInitialize()
        {
            Pool = new ObjectPool<Producer>(ObjPrefab, ParentTr, 8);
            _allProducers = new List<Producer>(8);
        }
    
        public Producer GenerateProducer(ProducerType producerType, int producerLevel, Vector2Int producerCoordinate)
        {
            var item = CreateObj();
            item.SetAttributes(producerCoordinate, producerType, producerLevel);
            item.ApplyData(ProducerDataDict[producerType].ProducerLevelDataDict[producerLevel]);
            return item;
        }
        
        public override Producer CreateObj()
        {
            var item = base.CreateObj();
            _allProducers.Add(item);
            return item;
        }

        public override void DestroyObj(Producer emptyItem)
        {
            base.DestroyObj(emptyItem);
            emptyItem.SetAttributes(-Vector2Int.one, ProducerType.None, 0);
            _allProducers.Remove(emptyItem);
        }

        public void DestroyAllProducers()
        {
            var itemsToDestroy = new List<Producer>(_allProducers);
            base.DestroyObjs(itemsToDestroy);
            _allProducers.Clear();
        }

    }
}