using System;
using Core.GridPawns.Data;
using Core.GridPawns.Enum;
using UnityEngine;

namespace Core.GridPawns
{
    public class Producer : GridPawn
    {
        [field: SerializeField] public ProducerType ProducerType { get; set; }

        public override System.Enum Type
        {
            get => ProducerType;
            protected set => ProducerType = (ProducerType)value;
        }

        public override void ApplyData(GridPawnLevelDataSO levelData)
        {
            base.ApplyData(levelData);
            var producerData = levelData as ProducerLevelDataSO;
            if (producerData is null)
            {
                throw new InvalidOperationException("Invalid data type provided!");
            }

            SpriteRenderer.sprite = producerData.ProducerSprite;
            //TODO: Get other things too
        }

        public override string ToString()
        {
            return $"Column{Coordinate.x},Row{Coordinate.y}, Level:{Level}, Type:{ProducerType}";
        }
    }
}
