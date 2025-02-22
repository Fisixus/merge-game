using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;


[CreateAssetMenu(fileName = "ApplianceData_00", menuName = "Grid Pawns/New ApplianceData")]
public class ApplianceDataSO : ScriptableObject
{
    [field: SerializeField]
    [SerializedDictionary("Appliance Level", "Level Data")]
    public SerializedDictionary<int, ApplianceLevelDataSO> ApplianceLevelDataDict { get; private set; }
}
