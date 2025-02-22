using AYellowpaper.SerializedCollections;
using Core.GridPawns.Enum;
using UnityEngine;

namespace Core.GridPawns.Data
{
    [CreateAssetMenu(fileName = "ProducerLevelData_00", menuName = "Grid Pawns/New ProducerLevelData")]
    public class ProducerLevelDataSO : GridPawnLevelDataSO
    {
        [field: SerializeField] public Sprite ProducerSprite { get; private set; }
        [field: SerializeField] public ApplianceType GeneratedApplianceType { get; private set; } //TODO: Could be more than one ApplianceType for future

        [field: SerializeField]
        [SerializedDictionary("Appliance Level", "Generation Ratio")]
        public SerializedDictionary<int, float> GeneratingRatioDict { get; private set; }
    }
}