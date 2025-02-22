using UnityEngine;

namespace MVP.Views.Interface
{
    public interface IGridView
    {
        SpriteRenderer GridSprite { get; }

        Transform GridTopLeftTr { get; }

        Vector2 CellSize { get; }

        Vector2 GridTopLeftMargin { get; }

        Vector2 GridPadding { get; }

        void CalculateGridSize(Vector2Int gridSize);

        void Scale(Vector2Int gridSize);
    }
}