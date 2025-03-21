using Core.GridPawns;
using UnityEngine;

namespace Core.Helpers
{
    public static class GridPawnModifierHelper
    {
        public static void SwapItems(GridPawn[,] grid, int x1, int y1, int x2, int y2)
        {
            // Swap the references in the grid array
            (grid[x1, y1], grid[x2, y2]) = (grid[x2, y2], grid[x1, y1]);

            // Update attributes only if the item is not null
            if (grid[x1, y1] != null)
                grid[x1, y1].SetAttributes(new Vector2Int(x1, y1), grid[x1, y1].Type, grid[x1, y1].Level,
                    grid[x1, y1].MaxLevel);


            if (grid[x2, y2] != null)
                grid[x2, y2].SetAttributes(new Vector2Int(x2, y2), grid[x2, y2].Type, grid[x2, y2].Level,
                    grid[x2, y2].MaxLevel);
        }
    }
}