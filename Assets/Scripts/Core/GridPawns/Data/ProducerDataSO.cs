using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Core.GridPawns.Data
{
    [CreateAssetMenu(fileName = "ProducerData_00", menuName = "Grid Pawns/New ProducerData")]
    public class ProducerDataSO : ScriptableObject
    {
        [field: SerializeField]
        [SerializedDictionary("Producer Level", "Level Data")]
        public SerializedDictionary<int, ProducerLevelDataSO> ProducerLevelDataDict { get; private set; }
    }
}