using UnityEngine;

namespace DI
{
    public abstract class Installer : MonoBehaviour
    {
        protected Container Container { get; private set; }

        public void Install(Container container)
        {
            Container = container;
            InstallBindings();
        }

        protected abstract void InstallBindings();
    }
}