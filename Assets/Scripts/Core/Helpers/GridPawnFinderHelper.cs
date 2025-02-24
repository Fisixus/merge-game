using System;
using Core.GridPawns;
using Core.GridPawns.Enum;

namespace Core.Helpers
{
    public static class GridPawnFinderHelper
    {
        public static GridPawn FindGridPawn(GridPawn[,] grid, Enum type, int level)
        {
            int columnCount = grid.GetLength(0);
            int rowCount = grid.GetLength(1);

            for (int x = 0; x < columnCount; x++)
            {
                for (int y = 0; y < rowCount; y++)
                {
                    GridPawn pawn = grid[x, y]; 
                    if (pawn != null && pawn.Type.Equals(type) && pawn.Level == level)
                    {
                        return pawn; //  Return first match found
                    }
                }
            }

            return null; //  No match found
        }

    }
}
