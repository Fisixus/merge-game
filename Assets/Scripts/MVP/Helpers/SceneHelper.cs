using DI.Contexts;
using UnityEngine.SceneManagement;

namespace MVP.Helpers
{
    public static class SceneHelper
    {
        /// <summary>
        /// Finds the SceneContext in the active scene by checking root game objects.
        /// </summary>
        /// <returns>The SceneContext if found, otherwise null.</returns>
        public static SceneContext FindSceneContextInActiveScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            foreach (var rootGameObject in activeScene.GetRootGameObjects())
            {
                var context = rootGameObject.GetComponent<SceneContext>();
                if (context != null)
                {
                    return context;
                }
            }

            return null;
        }

        /// <summary>
        /// Activates or deactivates all root game objects in the loading scene.
        /// </summary>
        /// <param name="loadingSceneName">The name of the loading scene.</param>
        /// <param name="isActive">True to activate, false to deactivate.</param>
        public static void SetLoadingSceneActive(string loadingSceneName, bool isActive)
        {
            var loadingScene = SceneManager.GetSceneByName(loadingSceneName);
            if (loadingScene.isLoaded)
            {
                foreach (var rootGameObject in loadingScene.GetRootGameObjects())
                {
                    rootGameObject.SetActive(isActive);
                }
            }
        }
    }
}