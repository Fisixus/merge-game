using System.Collections.Generic;
using UnityEngine;

namespace Core.Helpers
{
    public static class GridPositionHelper
    {
        public static readonly Dictionary<Vector2Int, Vector3> CoordinateToWorldPosDict = new();

        public static Vector3 CalculateItemWorldPosition(Vector3 gridTopLeftPosition, Vector2 longestCell,
            Vector2Int coordinate, float scaleFactor)
        {
            // Return cached value if exists
            if (CoordinateToWorldPosDict.TryGetValue(coordinate, out Vector3 cachedPosition))
                return cachedPosition;

            // Calculate new position if not cached
            Vector3 worldPosition = new Vector3(
                gridTopLeftPosition.x + (coordinate.x + 1) * longestCell.x * scaleFactor,
                gridTopLeftPosition.y - coordinate.y * longestCell.y * scaleFactor,
                gridTopLeftPosition.z
            );

            // Cache the position for future use
            CoordinateToWorldPosDict[coordinate] = worldPosition;

            return worldPosition;
        }


        // Validity check for grid bounds
        public static bool IsPositionValid(int x, int y, int columnCount, int rowCount)
        {
            return x >= 0 && x < columnCount && y >= 0 && y < rowCount;
        }
    }
}
