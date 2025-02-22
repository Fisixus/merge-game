using AYellowpaper.SerializedCollections;
using Core.GridPawns;
using Core.GridPawns.Enum;
using UnityEngine;

namespace Core.Factories.Interface
{
    public interface IProducerFactory : IFactory<Producer>
    {
        SerializedDictionary<ProducerType, ProducerDataSO> ProducerDataDict { get; }
        Producer GenerateProducer(ProducerType producerType ,int producerLevel, Vector2Int producerCoordinate);
        void DestroyAllProducers();
    }
}
