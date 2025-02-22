using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplianceLevelData_00", menuName = "Grid Pawns/New ApplianceLevelData")]
public class ApplianceLevelDataSO : GridPawnLevelDataSO
{
    [field: SerializeField] public Sprite ApplianceSprite { get; private set; }
}
