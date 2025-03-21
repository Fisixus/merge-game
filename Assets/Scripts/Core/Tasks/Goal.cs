using System;
using Core.GridPawns.Enum;
using UnityEngine;

namespace Core.Tasks
{
    [Serializable]
    public class Goal
    {
        [field: SerializeField] public ApplianceType ApplianceType { get; set; }
        [field: SerializeField] public int Level { get; set; }
    }
}