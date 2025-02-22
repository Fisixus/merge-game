using AYellowpaper.SerializedCollections;
using Core.Factories.Pools;
using Core.GridPawns;
using Core.GridPawns.Enum;
using UnityEngine;

namespace Core.Factories
{
    public class ApplianceFactory : ObjectFactory<Appliance>, IApplianceFactory
    {
    
        [field: SerializeField]
        [SerializedDictionary("Appliance Type", "Appliance Data")]
        public SerializedDictionary<ApplianceType, ApplianceDataSO> ApplianceDataDict { get; private set; }

        public override void PreInitialize()
        {
            Pool = new ObjectPool<Appliance>(ObjPrefab, ParentTr, 32);
        }
    
        public Appliance GenerateAppliance(ApplianceType applianceType, int applianceLevel, Vector2Int applianceCoordinate)
        {
            var item = CreateObj();
            item.SetAttributes(applianceCoordinate, applianceType, applianceLevel);
            item.ApplyData(ApplianceDataDict[applianceType].ApplianceLevelDataDict[applianceLevel]);
            return item;
        }

    }
}
