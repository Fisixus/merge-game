using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Core.GridPawns.Data
{
    [CreateAssetMenu(fileName = "ApplianceData_00", menuName = "Grid Pawns/New ApplianceData")]
    public class ApplianceDataSO : ScriptableObject
    {
        [field: SerializeField]
        [SerializedDictionary("Appliance Level", "Level Data")]
        public SerializedDictionary<int, ApplianceLevelDataSO> ApplianceLevelDataDict { get; private set; }
    }
}