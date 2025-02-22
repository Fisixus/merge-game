using UnityEngine;

namespace DI.Contexts
{
    public class ProjectContext : MonoBehaviour
    {
        public static Container Container { get; private set; } = new();

        [SerializeField] private Installer[] _installers;

        private void Awake()
        {
            foreach (var installer in _installers)
            {
                installer.Install(Container);
            }
        }
    }
}