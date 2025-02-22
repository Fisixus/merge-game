using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Core.Factories.Interface;
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
        private List<Appliance> _allAppliances;
        
        public override void PreInitialize()
        {
            Pool = new ObjectPool<Appliance>(ObjPrefab, ParentTr, 32);
            _allAppliances = new List<Appliance>(32);
        }
    
        public Appliance GenerateAppliance(ApplianceType applianceType, int applianceLevel, Vector2Int applianceCoordinate)
        {
            var item = CreateObj();
            item.SetAttributes(applianceCoordinate, applianceType, applianceLevel);
            item.ApplyData(ApplianceDataDict[applianceType].ApplianceLevelDataDict[applianceLevel]);
            return item;
        }
        
        public override Appliance CreateObj()
        {
            var item = base.CreateObj();
            _allAppliances.Add(item);
            return item;
        }

        public override void DestroyObj(Appliance emptyItem)
        {
            base.DestroyObj(emptyItem);
            emptyItem.SetAttributes(-Vector2Int.one, ApplianceType.None, 0);
            _allAppliances.Remove(emptyItem);
        }

        public void DestroyAllAppliances()
        {
            var itemsToDestroy = new List<Appliance>(_allAppliances);
            base.DestroyObjs(itemsToDestroy);
            _allAppliances.Clear();
        }

    }
}
