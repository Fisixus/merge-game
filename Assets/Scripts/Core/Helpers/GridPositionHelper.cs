using UnityEngine;

namespace Core.Helpers
{
    public static class GridPositionHelper
    {
        public static Vector3 CalculateItemWorldPosition(Vector3 gridTopLeftPosition, Vector2 longestCell,
            Vector2Int coordinate, float scaleFactor)
        {
            return new Vector3
            (
                gridTopLeftPosition.x + scaleFactor * longestCell.x * (coordinate.x + 1),
                gridTopLeftPosition.y - scaleFactor * longestCell.y * coordinate.y
            );
        }

        // Validity check for grid bounds
        public static bool IsPositionValid(int x, int y, int columnCount, int rowCount)
        {
            return x >= 0 && x < columnCount && y >= 0 && y < rowCount;
        }
    }
}
