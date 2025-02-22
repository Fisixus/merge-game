using System.Collections.Generic;
using Core.GridPawns;
using Core.GridSerialization;
using MVP.Models.Interface;
using MVP.Presenters.Handlers;

namespace MVP.Models
{
    public class GridModel : IGridModel
    {
        private readonly GridPawnFactoryHandler _gridPawnFactoryHandler;
        public GridPawn[,] Grid { get; private set; } // x:column, y:row
        public int ColumnCount { get; private set; }
        public int RowCount { get; private set; }

        public GridModel(GridPawnFactoryHandler gridPawnFactoryHandler)
        {
            _gridPawnFactoryHandler = gridPawnFactoryHandler;
        }
        
        private void Initialize(List<GridPawn> gridObjs, int columns, int rows)
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

        public void LoadGrid()
        {
            var gridInfo = GridSerializer.SerializeToGridInfo();
            // Process the grid in a single loop
            var gridPawnLevels = gridInfo.GridPawnLevels;
            var gridPawnTypes = gridInfo.GridPawnTypes;
            var cols = gridPawnTypes.GetLength(0);
            var rows = gridPawnTypes.GetLength(1);
            
            List<GridPawn> gridPawns = new List<GridPawn>(64);
            _gridPawnFactoryHandler.DestroyAllGridPawns();
            _gridPawnFactoryHandler.PopulateGridWithPawns(gridPawnTypes, gridPawnLevels, gridPawns);
            Initialize(gridPawns, cols, rows);
        }

        public void SaveGrid()
        {
            GridSerializer.SaveGrid(Grid);
        }
    }
}
