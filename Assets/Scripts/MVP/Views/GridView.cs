using MVP.Views.Interface;
using UnityEngine;

namespace MVP.Views
{
    public class GridView : MonoBehaviour, IGridView
    {
        [field: SerializeField] public SpriteRenderer GridSprite { get; private set; }
        [field: SerializeField] public Transform GridTopLeftTr { get; private set; }
        [field: SerializeField] public Vector2 CellSize { get; private set; }
        [field: SerializeField] public Vector2 GridTopLeftMargin { get; private set; }
        [field: SerializeField] public Vector2 GridPadding { get; private set; }

        private const float CamScalingFactor = 12f; // Scaling factor

        [field: SerializeField] public Camera Cam { get; private set; }

        private float _camDefaultSize;

        private void Awake()
        {
            _camDefaultSize = (float)Screen.width / Screen.height;
        }

        public void CalculateGridSize(Vector2Int gridSize)
        {
            ResetCamScale();
            var cellHeight = CellSize.y;
            var cellWidth = CellSize.x;

            // Calculate scaled padding based on grid size
            var paddingFactorX = 1f / gridSize.y;
            var paddingFactorY = 1f / gridSize.x;

            var basePadding = GridPadding;
            var scaledPadding = new Vector2(basePadding.x * paddingFactorX, basePadding.y * paddingFactorY);
            GridSprite.size = new Vector2((cellWidth + scaledPadding.x) * gridSize.y,
                (cellHeight + scaledPadding.y) * gridSize.x);
            UpdateGridTopLeftTr();
        }

        public void Scale(Vector2Int gridSize)
        {
            ResetCamScale();
            int gridHeight = gridSize.y;
            const float aspectRatio = (float)9 / 16;
            Cam.orthographicSize = (gridHeight / 2f) + (CamScalingFactor * aspectRatio);
        }

        private void ResetCamScale()
        {
            Cam.orthographicSize = _camDefaultSize;
        }

        private void UpdateGridTopLeftTr()
        {
            var bounds = GridSprite.bounds;
            GridTopLeftTr.position = new Vector2(bounds.min.x, bounds.max.y) + GridTopLeftMargin;
        }
    }
}