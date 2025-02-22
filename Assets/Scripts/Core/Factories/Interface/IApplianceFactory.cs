using AYellowpaper.SerializedCollections;
using Core.GridPawns;
using Core.GridPawns.Enum;
using UnityEngine;

namespace Core.Factories.Interface
{
    public interface IApplianceFactory : IFactory<Appliance>
    {
        SerializedDictionary<ApplianceType, ApplianceDataSO> ApplianceDataDict { get; }
        Appliance GenerateAppliance(ApplianceType applianceType ,int applianceLevel, Vector2Int applianceCoordinate);

        void DestroyAllAppliances();
    }
}
