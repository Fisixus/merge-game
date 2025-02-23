using System;
using System.Collections.Generic;
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
        event Action<GridPawn> OnGridPawnInitialized;
        event Action<GridPawn, Vector2Int?, bool> OnGridPawnUpdated;

        void UpdateGridPawns(List<GridPawn> gridPawns, Vector2Int? creationCoord, bool isAnimOn);
    }
}
