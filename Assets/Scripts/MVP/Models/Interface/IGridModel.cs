using System;
using System.Collections.Generic;
using Core.GridPawns;
using Core.GridSerialization;

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
        event Action<GridPawn> OnGridPawnInitializedEvent;

    }
}
