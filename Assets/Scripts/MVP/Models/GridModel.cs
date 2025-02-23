using System;
using System.Collections.Generic;
using Core.GridPawns;
using Core.GridPawns.Enum;
using Core.GridSerialization;
using MVP.Models.Interface;
using MVP.Presenters.Handlers;
using UnityEngine;

namespace MVP.Models
{
    public class GridModel : IGridModel
    {
        private readonly GridPawnFactoryHandler _gridPawnFactoryHandler;
        public GridPawn[,] Grid { get; private set; } // x:column, y:row
        public int ColumnCount { get; private set; }
        public int RowCount { get; private set; }
        
        public event Action<GridPawn> OnGridPawnInitialized;
        public event Action<GridPawn, Vector2Int?, bool> OnGridPawnUpdated;

        public GridModel(GridPawnFactoryHandler gridPawnFactoryHandler)
        {
            _gridPawnFactoryHandler = gridPawnFactoryHandler;
        }
        
        private void Initialize(List<GridPawn> gridObjs)
        {
            Grid = new GridPawn[ColumnCount, RowCount];
            for (var i = 0; i < ColumnCount; i++)
            for (var j = 0; j < RowCount; j++)
            {
                Grid[i, j] = gridObjs[i * RowCount + j];
                OnGridPawnInitialized?.Invoke(Grid[i, j]);
            }
        }
        public void UpdateGridPawns(List<GridPawn> newGridPawns, Vector2Int? creationCoord, bool isAnimationOn)
        {
            foreach (var newGridPawn in newGridPawns)
            {
                Grid[newGridPawn.Coordinate.x, newGridPawn.Coordinate.y] = newGridPawn;
                OnGridPawnUpdated?.Invoke(newGridPawn, creationCoord, isAnimationOn);
            }
        }
        

        public GridInfo GetGridInfo()
        {
            var gridInfo = GridSerializer.SerializeToGridInfo();
            if (gridInfo == null)
            {
                ColumnCount = 8;
                RowCount = 8;
            }
            return gridInfo;
        }

        public void LoadGrid(GridInfo gridInfo)
        {
            if (gridInfo == null) //First time entering
            {
                CreateGridFirstTime();
                return;
            }
            // Process the grid in a single loop
            var gridPawnLevels = gridInfo.GridPawnLevels;
            var gridPawnTypes = gridInfo.GridPawnTypes;
            
            ColumnCount = gridPawnTypes.GetLength(0);
            RowCount = gridPawnTypes.GetLength(1);
            
            List<GridPawn> gridPawns = new List<GridPawn>(64);
            _gridPawnFactoryHandler.DestroyAllGridPawns();
            _gridPawnFactoryHandler.PopulateGridWithPawns(gridPawnTypes, gridPawnLevels, gridPawns);
            Initialize(gridPawns);
        }

        public void SaveGrid()
        {
            GridSerializer.SaveGrid(Grid);
        }

        private void CreateGridFirstTime()
        {
            var gridPawnLevels = new int[ColumnCount,RowCount];
            var gridPawnTypes = new Enum[ColumnCount,RowCount];
            for (int i = 0; i < gridPawnLevels.GetLength(0); i++)
            {
                for (int j = 0; j < gridPawnLevels.GetLength(1); j++)
                {
                    if (j < 5 && i == 4)
                    {
                        gridPawnTypes[i,j] = ProducerType.ProducerA;
                        gridPawnLevels[i,j] = 1;
                    }
                    else
                    {
                        gridPawnTypes[i,j] = ApplianceType.None;
                        gridPawnLevels[i,j] = 0;
                    }
                }
            }
            
            List<GridPawn> gridPawns = new List<GridPawn>(64);
            _gridPawnFactoryHandler.DestroyAllGridPawns();
            _gridPawnFactoryHandler.PopulateGridWithPawns(gridPawnTypes, gridPawnLevels, gridPawns);
            
            Initialize(gridPawns);


        }
    }
}
