using System;
using Core.GridPawns;
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

            _gridModel.OnGridPawnInitialized += GridPawnInitialized;
            _gridModel.OnGridPawnUpdated += GridPawnUpdated;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
            Dispose();
        }
        private void Dispose()
        {
            _gridModel.OnGridPawnInitialized -= GridPawnInitialized;
            _gridModel.OnGridPawnUpdated -= GridPawnUpdated;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
        
        public async UniTask LoadGrid()
        {
            // TODO: Addressable logic next step
            var gridInfo = _gridModel.GetGridInfo();
            Vector2Int gridSize = gridInfo?.GridSize ?? new Vector2Int(_gridModel.ColumnCount, _gridModel.RowCount);

            _gridView.CalculateGridSize(gridSize);
            _gridModel.LoadGrid(gridInfo);

            // TODO: _goalHandler.Initialize(levelInfo.Goals, levelInfo.NumberOfMoves);
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
       
        private void GridPawnInitialized(GridPawn obj)
        {
            obj.SetWorldPosition(_gridView.CellSize, _gridView.GridTopLeftTr);
        }
        
        private void GridPawnUpdated(GridPawn obj, Vector2Int? newCoord, bool isAnimOn, float animTime)
        {
            obj.SetWorldPosition(_gridView.CellSize, _gridView.GridTopLeftTr, newCoord);
            obj.SetWorldPosition(_gridView.CellSize, _gridView.GridTopLeftTr, null, true, animTime);
        }
    }
}
