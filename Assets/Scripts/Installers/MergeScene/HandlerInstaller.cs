using DI;
using MVP.Presenters;
using MVP.Presenters.Handlers;

namespace Installers.MergeScene
{
    public class HandlerInstaller : Installer
    {
        protected override void InstallBindings()
        {
            Container.BindAsSingle(() => Container.Construct<GridPawnFactoryHandler>());
            Container.BindAsSingle(() => Container.Construct<DisappearEffectHandler>());
            Container.BindAsSingle(() => Container.Construct<MergeGlowEffectHandler>());
            Container.BindAsSingle(() => Container.Construct<TaskHandler>());
            
        }
    }
}