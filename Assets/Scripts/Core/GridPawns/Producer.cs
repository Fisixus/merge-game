using System;
using System.Collections;
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
        
        public Dictionary<int, float> GeneratingRatioDict { get; private set; }
        private int _maxCapacity;
        
        private Coroutine _capacityCoroutine;

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
            _maxCapacity = Capacity;
            GeneratedApplianceType = producerData.GeneratedApplianceType;
            GeneratingRatioDict = producerData.GeneratingRatioDict;
        }
        
        private void StartCapacityIncrease()
        {
            if (_capacityCoroutine == null)
            {
                _capacityCoroutine = StartCoroutine(IncreaseCapacityOverTime());
            }
        }

        private void StopCapacityIncrease()
        {
            if (_capacityCoroutine != null)
            {
                StopCoroutine(_capacityCoroutine);
                _capacityCoroutine = null;
            }
        }

        private IEnumerator IncreaseCapacityOverTime()
        {
            while (Capacity < _maxCapacity)
            {
                yield return new WaitForSeconds(30f); // Wait 30 seconds

                if (Capacity < _maxCapacity)
                {
                    Capacity++;
                    Debug.Log($"Capacity increased to: {Capacity}");

                    // Stop the coroutine when MaxCapacity is reached
                    if (Capacity >= _maxCapacity)
                    {
                        StopCapacityIncrease();
                    }
                }
            }
        }
        
        public void ReduceCapacity()
        {
            Capacity = Mathf.Max(0, --Capacity);
            Debug.Log($"Capacity reduced to: {Capacity}");

            if (Capacity < _maxCapacity)
            {
                StartCapacityIncrease(); // Restart capacity increase if needed
            }
        }

        public override string ToString()
        {
            return $"Column{Coordinate.x},Row{Coordinate.y}, Level:{Level}, Type:{ProducerType}";
        }
    }
}
