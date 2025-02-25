using Core.Factories.Interface;
using Core.Factories.Pools;
using Core.Tasks;

namespace Core.Factories
{
    public class TaskUIFactory : ObjectFactory<TaskUI>, ITaskUIFactory
    {
        public override void PreInitialize()
        {
            Pool = new ObjectPool<TaskUI>(ObjPrefab, ParentTr, 4);
        }
    }
}