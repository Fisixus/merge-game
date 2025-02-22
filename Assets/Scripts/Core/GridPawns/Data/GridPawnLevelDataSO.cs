using UnityEngine;

public abstract class GridPawnLevelDataSO : ScriptableObject
{
    [field: SerializeField] public Vector2 GridPawnWidthHeight { get; private set; }
}
