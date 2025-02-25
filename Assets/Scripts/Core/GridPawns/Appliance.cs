using System;
using Core.GridPawns.Data;
using Core.GridPawns.Effect;
using Core.GridPawns.Enum;
using UnityEngine;

namespace Core.GridPawns
{
    public class Appliance : GridPawn
    {
        [field: SerializeField] public ApplianceType ApplianceType { get; set; }

        // Override PawnEffect to return ApplianceEffect
        public new ApplianceEffect PawnEffect => (ApplianceEffect)base.PawnEffect;

        public override System.Enum Type
        {
            get => ApplianceType;
            protected set => ApplianceType = (ApplianceType)value;
        }

        public override void ApplyData(GridPawnLevelDataSO levelData)
        {
            base.ApplyData(levelData);
            var applianceData = levelData as ApplianceLevelDataSO;
            if (applianceData is null)
            {
                throw new InvalidOperationException("Invalid data type provided!");
            }

            SpriteRenderer.sprite = applianceData.ApplianceSprite;
        }

        public override string ToString()
        {
            return $"Column{Coordinate.x},Row{Coordinate.y}, Level:{Level}, Type:{ApplianceType}";
        }
    }
}