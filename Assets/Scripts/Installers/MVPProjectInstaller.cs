using DI;
using MVP.Presenters;
using MVP.Presenters.Handlers;

namespace Installers
{
    public class MVPProjectInstaller : Installer
    {
        protected override void InstallBindings()
        {
            Container.BindAsSingle(() => Container.Construct<ScenePresenter>());
            Container.BindAsSingle(() => Container.Construct<SceneTransitionHandler>());
            Container.BindAsSingleNonLazy(() => Container.Construct<GamePresenter>());
        }
    }
}
