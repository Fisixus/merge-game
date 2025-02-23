using Core.Factories;
using Core.Factories.Interface;
using DI;
using UnityEngine;

namespace Installers.MergeScene
{
    public class FactoryInstaller : Installer
    {
        // Reference to the scene object
        [SerializeField] private ApplianceFactory _applianceFactory;
        [SerializeField] private ProducerFactory _producerFactory;
        [SerializeField] private DisappearEffectFactory _disappearEffectFactory;

        protected override void InstallBindings()
        {
            Container.BindAsSingle<IApplianceFactory>(() => _applianceFactory);
            Container.BindAsSingle<IProducerFactory>(() => _producerFactory);
            Container.BindAsSingle<IDisappearEffectFactory>(() => _disappearEffectFactory);
        }
    }
}