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

        public override void PreInitialize()
        {
            Pool = new ObjectPool<Producer>(ObjPrefab, ParentTr, 8);
        }
    
        public Producer GenerateProducer(ProducerType producerType, int producerLevel, Vector2Int producerCoordinate)
        {
            var item = CreateObj();
            item.SetAttributes(producerCoordinate, producerType, producerLevel);
            item.ApplyData(ProducerDataDict[producerType].ProducerLevelDataDict[producerLevel]);
            return item;
        }

    }
}