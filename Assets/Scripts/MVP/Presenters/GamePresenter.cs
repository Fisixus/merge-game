using Cysharp.Threading.Tasks;
using MVP.Presenters.Handlers;
using UnityEngine;

namespace MVP.Presenters
{
    public class GamePresenter
    {
        private readonly ScenePresenter _scenePresenter;
        private readonly SceneTransitionHandler _sceneTransitionHandler;

        public GamePresenter(ScenePresenter scenePresenter, SceneTransitionHandler sceneTransitionHandler)
        {
            _scenePresenter = scenePresenter;
            _sceneTransitionHandler = sceneTransitionHandler;

            Application.targetFrameRate = 60;
            InitializeGame().Forget();
        }

        private async UniTask InitializeGame()
        {
            await _scenePresenter.TransitionToNextScene("MainScene",
                async (container) => { await _sceneTransitionHandler.SetupMainSceneRequirements(container); });
        }
    }
}