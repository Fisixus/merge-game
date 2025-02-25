using Cysharp.Threading.Tasks;
using DI;

namespace MVP.Presenters.Handlers
{
    public class SceneTransitionHandler
    {
        public async UniTask SetupMainSceneRequirements(Container container)
        {
            // Future async work can go here
            await UniTask.CompletedTask;
        }

        public async UniTask SetupMergeSceneRequirements(Container container)
        {
            GridPresenter gridPresenter = container.Resolve<GridPresenter>();
            await gridPresenter.LoadGrid();
            TaskPresenter taskPresenter = container.Resolve<TaskPresenter>();
            await taskPresenter.InitializeTasks();
        }
    }
}