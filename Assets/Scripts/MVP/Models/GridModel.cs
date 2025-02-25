using System;
using System.Collections.Generic;
using Core.GridPawns;
using Core.GridPawns.Enum;
using Core.GridSerialization;
using Core.Helpers;
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

        public event Action<Vector2Int> OnGridCoordinateToWorldPosCalculated;
        public event Action<GridPawn> OnGridPawnInitialized;
        public event Action<GridPawn, Vector2Int?, bool, float> OnGridPawnUpdated;

        public GridModel(GridPawnFactoryHandler gridPawnFactoryHandler)
        {
            _gridPawnFactoryHandler = gridPawnFactoryHandler;
        }

        private void Initialize(List<GridPawn> gridObjs)
        {
            Grid = new GridPawn[ColumnCount, RowCount];

            for (var i = 0; i < ColumnCount; i++)
            {
                for (var j = 0; j < RowCount; j++)
                {
                    var index = i * RowCount + j;

                    Grid[i, j] = gridObjs[index];
                    OnGridCoordinateToWorldPosCalculated?.Invoke(new Vector2Int(i, j));

                    if (Grid[i, j] != null)
                    {
                        OnGridPawnInitialized?.Invoke(Grid[i, j]);
                    }
                }
            }

            SaveGrid();
        }

        public void UpdateGridPawn(GridPawn gridPawn, bool isRemoving, Vector2Int? coordOverride = null,
            bool isAnimationOn = false, float animTime = 0f)
        {
            if (gridPawn == null) return; // Ensure gridPawn is valid

            var coord = gridPawn.Coordinate;

            // Update the grid based on whether we are removing or setting a new pawn
            Grid[coord.x, coord.y] = isRemoving ? null : gridPawn;

            if (!isRemoving)
            {
                OnGridPawnUpdated?.Invoke(gridPawn, coordOverride, isAnimationOn, animTime);
            }

            SaveGrid();
        }


        public void SwapGridItems(GridPawn firstPawn, Vector2Int secondCoord)
        {
            var firstCoord = firstPawn.Coordinate;

            // Swap the pawns even if one is null (to move into an empty space)
            GridPawnModifierHelper.SwapItems(Grid, firstCoord.x, firstCoord.y, secondCoord.x, secondCoord.y);
            SaveGrid();
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
            var gridPawnCapacities = gridInfo.GridPawnCapacities;


            ColumnCount = gridPawnTypes.GetLength(0);
            RowCount = gridPawnTypes.GetLength(1);

            List<GridPawn> gridPawns = new List<GridPawn>(64);
            _gridPawnFactoryHandler.DestroyAllGridPawns();
            _gridPawnFactoryHandler.PopulateGridWithPawns(gridPawnTypes, gridPawnLevels, gridPawnCapacities, gridPawns);
            Initialize(gridPawns);
        }

        public void SaveGrid()
        {
            GridSerializer.SaveGrid(Grid);
        }

        private void CreateGridFirstTime()
        {
            var gridPawnLevels = new int[ColumnCount, RowCount];
            var gridPawnTypes = new Enum[ColumnCount, RowCount];
            var gridPawnCapacities = new int[ColumnCount, RowCount];

            for (int i = 0; i < gridPawnLevels.GetLength(0); i++)
            {
                for (int j = 0; j < gridPawnLevels.GetLength(1); j++)
                {
                    if (j < 5 && i == 4)
                    {
                        gridPawnTypes[i, j] = ProducerType.ProducerA;
                        gridPawnLevels[i, j] = 1;
                        gridPawnCapacities[i, j] = 10;
                    }
                    else
                    {
                        gridPawnTypes[i, j] = ApplianceType.None;
                        gridPawnLevels[i, j] = 0;
                        gridPawnCapacities[i, j] = -1;
                    }
                }
            }

            List<GridPawn> gridPawns = new List<GridPawn>(64);
            _gridPawnFactoryHandler.DestroyAllGridPawns();
            _gridPawnFactoryHandler.PopulateGridWithPawns(gridPawnTypes, gridPawnLevels, gridPawnCapacities, gridPawns);

            Initialize(gridPawns);
        }
    }
}