using System;
using System.Collections;
using System.Collections.Generic;
using Core.GridPawns.Data;
using Core.GridPawns.Effect;
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
        
        // Override PawnEffect to return ProducerEffect
        public new ProducerEffect PawnEffect => (ProducerEffect)base.PawnEffect;

        private Dictionary<int, float> _generatingRatioDict { get; set; }
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
            _generatingRatioDict = producerData.GeneratingRatioDict;
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
                    //Debug.Log($"Capacity increased to: {Capacity}");

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
            //Debug.Log($"Capacity reduced to: {Capacity}");

            if (Capacity != 0 && Capacity < _maxCapacity)
            {
                StartCapacityIncrease(); // Restart capacity increase if needed
            }
            
        }

        public override string ToString()
        {
            return $"Column{Coordinate.x},Row{Coordinate.y}, Level:{Level}, Type:{ProducerType}";
        }

        /// <summary> Selects an appliance level to produce based on the probability ratios. </summary>
        public int GetApplianceLevelToProduce()
        {
            if (_generatingRatioDict == null || _generatingRatioDict.Count == 0)
            {
                Debug.LogError("GeneratingRatioDict is empty! Returning default level 1.");
                return 1; // Default level
            }

            float randomValue = UnityEngine.Random.value; // Random float between 0.0 - 1.0
            float cumulativeProbability = 0f;

            foreach (var kvp in _generatingRatioDict)
            {
                cumulativeProbability += kvp.Value;

                if (randomValue <= cumulativeProbability)
                {
                    return kvp.Key; // Return selected appliance level
                }
            }

            Debug.LogError("No valid appliance level found in GeneratingRatioDict. Returning default.");
            return 1; // Fallback default level
        }
    }
}
