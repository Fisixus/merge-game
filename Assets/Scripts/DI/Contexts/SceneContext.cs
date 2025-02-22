using UnityEngine;

namespace DI.Contexts
{
    public class SceneContext : MonoBehaviour
    {
        public Container SceneContainer => _sceneContainer;
        private Container _sceneContainer = new();

        [SerializeField] private Installer[] _installers;

        private void Awake()
        {
            foreach (var installer in _installers)
            {
                installer.Install(_sceneContainer);
            }
        }
    }
}