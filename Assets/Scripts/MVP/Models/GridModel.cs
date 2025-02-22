using System.Collections.Generic;
using Core.GridPawns;
using MVP.Models.Interface;

namespace MVP.Models
{
    public class GridModel : IGridModel
    {
        public GridPawn[,] Grid { get; private set; } // x:column, y:row
        public int ColumnCount { get; private set; }
        public int RowCount { get; private set; }
        public void Initialize(List<GridPawn> gridObjs, int columns, int rows)
        {
            ColumnCount = columns;
            RowCount = rows;
            Grid = new GridPawn[ColumnCount, RowCount];
            for (var i = 0; i < ColumnCount; i++)
            for (var j = 0; j < RowCount; j++)
            {
                Grid[i, j] = gridObjs[i * RowCount + j];
                //TODO:OnGridObjectInitializedEvent?.Invoke(Grid[i, j]);
            }
        }
    }
}
