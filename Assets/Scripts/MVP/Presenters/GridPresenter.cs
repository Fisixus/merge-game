using System;
using Cysharp.Threading.Tasks;
using MVP.Models.Interface;
using MVP.Presenters.Handlers;
using MVP.Views.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVP.Presenters
{
    public class GridPresenter
    {
        private readonly IGridModel _gridModel;
        private readonly TaskHandler _taskHandler;
        private readonly IGridView _gridView;
        
        public GridPresenter(IGridModel gridModel, TaskHandler taskHandler, IGridView gridView)
        {
            _gridModel = gridModel;
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
            //TODO:Addressable logic next step
            
            _gridModel.LoadGrid();
            var gridSize = new Vector2Int(_gridModel.ColumnCount, _gridModel.RowCount);
            _gridView.CalculateGridSize(gridSize);
            //TODO: _goalHandler.Initialize(levelInfo.Goals, levelInfo.NumberOfMoves);
            _gridView.Scale(gridSize);
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
