using AYellowpaper.SerializedCollections;
using Core.Factories.Interface;
using Core.GridPawns;
using Core.GridPawns.Enum;
using UnityEditor;
using UnityEngine;

public interface IApplianceFactory : IFactory<Appliance>
{
    SerializedDictionary<ApplianceType, ApplianceDataSO> ApplianceDataDict { get; }
    Appliance GenerateAppliance(ApplianceType applianceType ,int applianceLevel, Vector2Int applianceCoordinate);
}
