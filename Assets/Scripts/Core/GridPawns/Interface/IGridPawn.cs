using UnityEngine;

namespace Core.GridPawns.Interface
{
    public interface IGridPawn
    {
        BoxCollider2D BoxCollider { get; }
        SpriteRenderer SpriteRenderer { get; }
        Vector2Int Coordinate { get; set; }

        // void SetWorldPosition(Vector2 longestCell, Transform gridTopLeftTr, Vector2Int? coordinateOverride = null,
        //     bool isAnimationOn = false, float animationTime = 0.2f);
        //
        // void SetAttributes(Vector2Int newCoord, Enum type);
    }
}
