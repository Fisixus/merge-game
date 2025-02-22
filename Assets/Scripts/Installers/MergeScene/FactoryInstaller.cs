using Core.Factories;
using Core.Factories.Interface;
using DI;
using UnityEngine;

namespace Installers.LevelScene
{
    public class FactoryInstaller : Installer
    {
        // Reference to the scene object
        [SerializeField] private ApplianceFactory _applianceFactory;
        [SerializeField] private ProducerFactory _producerFactory;

        protected override void InstallBindings()
        {
            Container.BindAsSingle<IApplianceFactory>(() => _applianceFactory);
            Container.BindAsSingle<IProducerFactory>(() => _producerFactory);
        }
    }
}