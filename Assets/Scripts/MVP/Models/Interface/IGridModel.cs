using System;
using Core.GridPawns;
using Core.GridSerialization;
using UnityEngine;

namespace MVP.Models.Interface
{
    public interface IGridModel
    {
        GridPawn[,] Grid { get; } // x:column, y:row
        int ColumnCount { get; }
        int RowCount { get; }
        GridInfo GetGridInfo();
        void LoadGrid(GridInfo gridInfo);
        void SaveGrid();
        event Action<Vector2Int> OnGridCoordinateToWorldPosCalculated;
        event Action<GridPawn> OnGridPawnInitialized;
        event Action<GridPawn, Vector2Int?, bool, float> OnGridPawnUpdated;

        void UpdateGridPawn(GridPawn newGridPawn, bool isRemoving, Vector2Int? coordOverride = null,
            bool isAnimOn = false, float animTime = 0.3f);

        void SwapGridItems(GridPawn firstPawn, Vector2Int secondCoord);
    }
}