using DI;
using MVP.Views;
using MVP.Views.Interface;
using UnityEngine;

namespace Installers.MainScene
{
    public class MVPMainInstaller : Installer
    {
        [SerializeField] private MainUIView _mainUIView;

        protected override void InstallBindings()
        {
            Container.BindAsSingle<IMainUIView>(() => _mainUIView);
        }
    }
}