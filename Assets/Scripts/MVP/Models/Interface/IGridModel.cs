using System.Collections.Generic;
using Core.GridPawns;

namespace MVP.Models.Interface
{
    public interface IGridModel
    {
        GridPawn[,] Grid { get; } // x:column, y:row
        int ColumnCount { get; }
        int RowCount { get; }
        void Initialize(List<GridPawn> gridObjs, int columns, int rows);

    }
}
