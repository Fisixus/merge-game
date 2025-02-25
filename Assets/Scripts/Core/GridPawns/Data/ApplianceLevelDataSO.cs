using UnityEngine;

namespace Core.GridPawns.Data
{
    [CreateAssetMenu(fileName = "ApplianceLevelData_00", menuName = "Grid Pawns/New ApplianceLevelData")]
    public class ApplianceLevelDataSO : GridPawnLevelDataSO
    {
        [field: SerializeField] public Sprite ApplianceSprite { get; private set; }
    }
}