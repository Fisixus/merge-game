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
    
        public Producer GenerateProducer(ProducerType producerType, int producerLevel, Vector2Int producerCoordinate, int capacityOverride = -1)
        {
            if (!ProducerDataDict.TryGetValue(producerType, out var producerData) ||
                !producerData.ProducerLevelDataDict.TryGetValue(producerLevel, out var levelData)) return null;
            var producer = CreateObj();
            producer.SetAttributes(producerCoordinate, producerType, producerLevel, producerData.ProducerLevelDataDict.Count);
            producer.ApplyData(levelData);
            if (capacityOverride != -1)
                producer.Capacity = capacityOverride;
            return producer;

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
            emptyItem.SetAttributes(emptyItem.Coordinate, ProducerType.None, -1, -1);
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