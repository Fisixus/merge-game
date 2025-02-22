using Core.GridPawns.Enum;
using Core.GridPawns.Interface;
using Core.Helpers;
using DG.Tweening;
using UnityEngine;

namespace Core.GridPawns
{
    public abstract class GridPawn : MonoBehaviour, IGridPawn, IType
    {
        [field: SerializeField] public BoxCollider2D BoxCollider { get; private set; }
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        [field: SerializeField] public SpriteRenderer FocusSprite { get; private set; }
        [field: SerializeField] public Vector2Int Coordinate { get; set; }
        
        [field: SerializeField] public int Level { get;  set; }
        
        public abstract System.Enum Type { get; protected set; } // Enforced by derived classes to follow IType
        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                _isEmpty = value;
                if (_isEmpty)
                {
                    // If empty, adjust other properties accordingly
                    SpriteRenderer.enabled = false;
                    BoxCollider.enabled = false;
                }
                else
                {
                    // If not empty, adjust properties oppositely
                    SpriteRenderer.enabled = true;
                    BoxCollider.enabled = true;
                }
            }
        }

        private bool _isEmpty;
        
        public void SetWorldPosition(Vector2 longestCell, Transform gridTopLeftTr, Vector2Int? coordinateOverride = null)
        {
            // Use the provided override coordinate or default to the current coordinate
            var targetCoordinate = coordinateOverride ?? Coordinate;

            // Calculate the world position
            var position = CalculateWorldPosition(longestCell, gridTopLeftTr, targetCoordinate);
            transform.position = position;
        }

        // Helper Method for Position Calculation
        private Vector3 CalculateWorldPosition(Vector2 longestCell, Transform gridTopLeftTr,
            Vector2Int targetCoordinate)
        {
            var gridTr = gridTopLeftTr.parent;
            var scaleNormalizing = gridTr.localScale.x;

            return GridPositionHelper.CalculateItemWorldPosition(gridTopLeftTr.position, longestCell, targetCoordinate,
                scaleNormalizing);
        }
        

        // SetAttributes, leveraging IType and polymorphism
        public void SetAttributes(Vector2Int newCoord, System.Enum type, int level)
        {
            Coordinate = newCoord;
            Type = type;
            Level = level;
            name = ToString();
            SetSortingOrder(-newCoord.y);

            // Update the GridAttributes
            IsEmpty = Type is ApplianceType.None or ProducerType.None;
        }

        public void SetSortingOrder(int order)
        {
            SpriteRenderer.sortingOrder = order;
        }

        public virtual void ApplyData(GridPawnLevelDataSO levelData)
        {
            var gridObjectWidthHeight = levelData.GridPawnWidthHeight;
            SpriteRenderer.size = new Vector2(gridObjectWidthHeight.x, gridObjectWidthHeight.y);
            BoxCollider.size = new Vector2(gridObjectWidthHeight.x, gridObjectWidthHeight.y);
        }

        public abstract override string ToString();
        
    }
}
