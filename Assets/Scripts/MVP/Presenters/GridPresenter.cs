using System;
using Cysharp.Threading.Tasks;
using MVP.Presenters.Handlers;
using MVP.Views.Interface;
using UnityEngine.SceneManagement;

namespace MVP.Presenters
{
    public class GridPresenter
    {
        private readonly GridSetupHandler _levelSetupHandler;
        private readonly TaskHandler _taskHandler;
        private readonly IGridView _gridView;
        
        public GridPresenter(GridSetupHandler levelSetupHandler, TaskHandler taskHandler, IGridView gridView)
        {
            _levelSetupHandler = levelSetupHandler;
            _taskHandler = taskHandler;
            _gridView = gridView;

            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
            Dispose();
        }
        private void Dispose()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
        
        public async UniTask LoadGrid()
        {
            // var levelInfo = levelModel.LoadLevel(); //TODO:Addressable logic next step
            // _gridView.CalculateGridSize(levelInfo.GridSize);
            // _levelSetupHandler.Initialize(levelInfo);
            // _goalHandler.Initialize(levelInfo.Goals, levelInfo.NumberOfMoves);
            // _gridView.Scale(levelInfo.GridSize);
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f), DelayType.DeltaTime);
        }

        // public void LoadFromGridEditor(LevelInfo levelInfo)
        // {
        //     _gridView.CalculateGridSize(levelInfo.GridSize);
        //     _levelSetupHandler.Initialize(levelInfo);
        //     _goalHandler.Initialize(levelInfo.Goals, levelInfo.NumberOfMoves);
        //     _gridView.Scale(levelInfo.GridSize);
        // }
       
    }
}
