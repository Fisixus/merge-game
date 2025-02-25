using System;
using Cysharp.Threading.Tasks;
using DI.Contexts;
using MVP.Presenters;
using MVP.Presenters.Handlers;
using MVP.Views.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.Views
{
    public class MainUIView : MonoBehaviour, IMainUIView
    {
        [field: SerializeField] public Button PlayButton { get; private set; }

        private void Awake()
        {
            PlayButton.onClick.AddListener(() => { RequestLevel().Forget(); });
        }

        private void OnDisable()
        {
            PlayButton.onClick.RemoveAllListeners();
        }

        private async UniTask RequestLevel()
        {
            // Disable the button to prevent multiple clicks
            PlayButton.interactable = false;

            // Resolve dependencies
            var scenePresenter = ProjectContext.Container.Resolve<ScenePresenter>();
            var mergeTransitionHandler = ProjectContext.Container.Resolve<SceneTransitionHandler>();
            // Perform scene transition
            await scenePresenter.TransitionToNextScene("MergeScene",
                async (container) =>
                {
                    // Specific setup logic for this scene
                    await mergeTransitionHandler.SetupMergeSceneRequirements(container);
                });
            try
            {
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }
    }
}