using System.Collections.Generic;
using Core.GridPawns;
using UnityEngine;

namespace Core.Helpers
{
    public static class GridPositionHelper
    {
        private static readonly Dictionary<Vector2Int, Vector3> CoordinateToWorldPosDict = new();
        private const float MaxDistanceThreshold = 0.5f; // Adjust as needed

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

        /// <summary>
        /// Finds the closest grid coordinate to the given release point if within a certain distance.
        /// </summary>
        public static Vector2Int? FindClosestCell(Vector3 releasePoint)
        {
            Vector2Int? closestCoord = null;
            float minDistance = MaxDistanceThreshold; // Start with threshold

            foreach (var kvp in CoordinateToWorldPosDict)
            {
                float distance = Vector3.Distance(kvp.Value, releasePoint);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCoord = kvp.Key;
                }
            }

            return closestCoord;
        }


        /// <summary> Finds the closest empty coordinate using a breadth-first search (BFS). </summary>
        public static Vector2Int? FindClosestEmptyCoordinate(Vector2Int origin, GridPawn[,] grid)
        {
            int columnCount = grid.GetLength(0);
            int rowCount = grid.GetLength(1);
            
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

            queue.Enqueue(origin);
            visited.Add(origin);

            // Define movement directions: Right, Left, Down, Up
            Vector2Int[] directions =
            {
                new Vector2Int(1, 0),  // Right
                new Vector2Int(-1, 0), // Left
                new Vector2Int(0, 1),  // Up
                new Vector2Int(0, -1)  // Down
            };

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();

                foreach (Vector2Int direction in directions)
                {
                    Vector2Int next = current + direction;

                    if (IsPositionValid(next, columnCount, rowCount) && !visited.Contains(next))
                    {
                        visited.Add(next);

                        if (grid[next.x, next.y].IsEmpty)
                        {
                            return next; // Return first found empty cell
                        }

                        queue.Enqueue(next); // Continue searching
                    }
                }
            }

            return null; // No empty coordinate found
        }

        /// <summary> Checks if a coordinate is within valid grid bounds. </summary>
        private static bool IsPositionValid(Vector2Int coord, int columnCount, int rowCount)
        {
            return coord.x >= 0 && coord.x < columnCount && coord.y >= 0 && coord.y < rowCount;
        }

    }
}
