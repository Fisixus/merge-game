using DI;
using UnityEngine;

namespace Installers.MainScene
{
    public class MainSceneInstaller : Installer
    {
        [SerializeField] private MVPMainInstaller mvpMainInstaller;

        protected override void InstallBindings()
        {
            mvpMainInstaller.Install(Container);
        }
    }
}