using System;
using UnityEngine;

namespace Core.GridSerialization
{
    public class GridInfo
    {
        public Enum[,] GridPawnTypes { get; private set; }
        public int[,] GridPawnLevels { get; private set; }
        public int[,] GridPawnCapacities { get; private set; }
        public Vector2Int GridSize { get; private set; }

        public GridInfo(Enum[,] gridPawnTypes, int[,] gridPawnLevels, int[,] gridPawnCapacities)
        {
            GridPawnTypes = gridPawnTypes;
            GridPawnLevels = gridPawnLevels;
            GridPawnCapacities = gridPawnCapacities;
            GridSize = new Vector2Int(GridPawnTypes.GetLength(0), GridPawnTypes.GetLength(1));
        }

        public override string ToString()
        {
            return $"GridSize:{GridSize}";
        }
    }
}