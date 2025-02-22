using System;
using System.Collections.Generic;
using Core.GridPawns.Data;
using Core.GridPawns.Enum;
using UnityEngine;

namespace Core.GridPawns
{
    public class Producer : GridPawn
    {
        [field: SerializeField] public SpriteRenderer CapacitySprite { get; private set; }
        
        [field: SerializeField] public ProducerType ProducerType { get; set; }
        [field: SerializeField] public int Capacity { get; set; }
        [field: SerializeField] public ApplianceType GeneratedApplianceType { get; set; }
        
        private Dictionary<int, float> _generatingRatioDict { get; set; }

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
            Capacity = producerData.Capacity;
            GeneratedApplianceType = producerData.GeneratedApplianceType;
            _generatingRatioDict = producerData.GeneratingRatioDict;
        }

        public override string ToString()
        {
            return $"Column{Coordinate.x},Row{Coordinate.y}, Level:{Level}, Type:{ProducerType}";
        }
    }
}
