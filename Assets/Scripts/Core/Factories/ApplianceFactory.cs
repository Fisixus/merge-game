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
            Pool = new ObjectPool<Appliance>(ObjPrefab, ParentTr, 64);
            _allAppliances = new List<Appliance>(64);
        }
    
        public Appliance GenerateAppliance(ApplianceType applianceType, int applianceLevel, Vector2Int applianceCoordinate)
        {
            var appliance = CreateObj();
            appliance.SetAttributes(applianceCoordinate, applianceType, applianceLevel);

            if (ApplianceDataDict.TryGetValue(applianceType, out var applianceData) &&
                applianceData.ApplianceLevelDataDict.TryGetValue(applianceLevel, out var levelData))
            {
                appliance.ApplyData(levelData);
            }
            else
            {
                DestroyObj(appliance);
            }

            return appliance;
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
            emptyItem.SetAttributes(emptyItem.Coordinate, ApplianceType.None, 0);//TODO:
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
