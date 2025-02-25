using Core.Factories.Interface;
using Core.Factories.Pools;
using Core.Inventories;

namespace Core.Factories
{
    public class InventoryPawnUIFactory : ObjectFactory<InventoryPawnUI>, IInventoryPawnUIFactory
    {
        public override void PreInitialize()
        {
            Pool = new ObjectPool<InventoryPawnUI>(ObjPrefab, ParentTr, 16);
        }
    }
}