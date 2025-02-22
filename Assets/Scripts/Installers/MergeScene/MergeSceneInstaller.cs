using DI;
using Installers.LevelScene;
using UnityEngine;

namespace Installers.MergeScene
{
    public class LevelSceneInstaller : Installer
    {
        [SerializeField] private HandlerInstaller _handlerInstaller;
        [SerializeField] private MVPMergeInstaller _mvpMergeInstaller;
        [SerializeField] private FactoryInstaller _factoryInstaller;

        protected override void InstallBindings()
        {
            _factoryInstaller.Install(Container);
            _handlerInstaller.Install(Container);
            _mvpMergeInstaller.Install(Container);
        }
    }
}