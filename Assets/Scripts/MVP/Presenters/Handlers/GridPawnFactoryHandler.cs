using System;
using System.Collections.Generic;
using Core.Factories.Interface;
using Core.GridPawns;
using Core.GridPawns.Enum;
using Core.Inventories;
using UnityEngine;

namespace MVP.Presenters.Handlers
{
    public class GridPawnFactoryHandler
    {
        private readonly IApplianceFactory _applianceFactory;
        private readonly IProducerFactory _producerFactory;

        public GridPawnFactoryHandler(IApplianceFactory applianceFactory, IProducerFactory producerFactory)
        {
            _applianceFactory = applianceFactory;
            _producerFactory = producerFactory;
        }
        
        public void PopulateGridWithPawns(Enum[,] gridPawnTypes, int[,] gridPawnLevels, List<GridPawn> gridPawns)
        {
            // Process the grid with column-to-row traversal
            for (int i = 0; i < gridPawnTypes.GetLength(1); i++) // Columns
            {
                for (int j = 0; j < gridPawnTypes.GetLength(0); j++) // Rows
                {
                    Vector2Int coordinate = new Vector2Int(i, j);
                    var gridType = gridPawnTypes[i, j];
                    var pawnLevel = gridPawnLevels[i, j];

                    // Encapsulated factory logic
                    var gridPawn = CreateGridPawn(gridType, pawnLevel, coordinate);
                    gridPawns.Add(gridPawn);
                }
            }
        }
        
        
        public void DestroyAllGridPawns()
        {
            _applianceFactory.DestroyAllAppliances();
            _producerFactory.DestroyAllProducers();
        }
        
        public void DestroyPawn(GridPawn pawn)
        {
            switch (pawn)
            {
                case Appliance appliance:
                    _applianceFactory.DestroyObj(appliance);
                    break;
                case Producer producer:
                    _producerFactory.DestroyObj(producer);
                    break;
                default:
                    Debug.LogWarning($"Unknown grid type: {pawn}");
                    break;
            }
        }

        public Sprite GetSprite(Enum type, int level)
        {
            return type switch
            {
                ApplianceType applianceType => GetApplianceSprite(applianceType, level),
                ProducerType producerType => GetProducerSprite(producerType, level),
                _ => LogUnknownTypeAndReturnNull(type)
            };
        }


        private Sprite GetApplianceSprite(ApplianceType type, int level)
        {
            return _applianceFactory.ApplianceDataDict[type].ApplianceLevelDataDict[level].ApplianceSprite;
        }

        private Sprite GetProducerSprite(ProducerType type, int level)
        {
            return _producerFactory.ProducerDataDict[type].ProducerLevelDataDict[level].ProducerSprite;
        }

        private Sprite LogUnknownTypeAndReturnNull(GridPawn pawn)
        {
            Debug.LogWarning($"Unknown grid type: {pawn}");
            return null;
        }
        private Sprite LogUnknownTypeAndReturnNull(Enum e)
        {
            Debug.LogWarning($"Unknown grid type: {e}");
            return null;
        }
        public GridPawn CreateGridPawn(Enum pawnType, int level, Vector2Int coordinate)
        {
            switch (pawnType)
            {
                case ApplianceType applianceType:
                    return _applianceFactory.GenerateAppliance(applianceType, level, coordinate);
                case ProducerType producerType:
                    return _producerFactory.GenerateProducer(producerType, level, coordinate);
                default:
                    Debug.LogWarning($"Unknown grid type: {pawnType} at {coordinate}");
                    return null;
            }
        }

        public Producer RecycleProducer(Producer producer, Vector2Int randomEmptyCoordinate)
        {
            var newProducer = _producerFactory.GenerateProducer(producer.ProducerType, producer.Level, randomEmptyCoordinate);
            _producerFactory.DestroyObj(producer);
            return newProducer;
        }

        public GridPawn MergePawns(GridPawn pawn1, GridPawn pawn2)
        {
            if (pawn1 == null || pawn2 == null)
            {
                Debug.LogWarning("Attempted to merge null pawns.");
                return null;
            }

            var newLevel = pawn1.Level + 1;
            var newCoordinate = pawn2.Coordinate;

            return pawn1 switch
            {
                Appliance appliance1 when pawn2 is Appliance appliance2 
                    => MergeAppliances(appliance1, appliance2, newLevel, newCoordinate),
                Producer producer1 when pawn2 is Producer producer2 
                    => MergeProducers(producer1, producer2, newLevel, newCoordinate),
                _ => HandleUnknownType(pawn1, newCoordinate)
            };
        }

        private GridPawn MergeAppliances(Appliance appliance1, Appliance appliance2, int newLevel, Vector2Int coordinate)
        {
            var newAppliance = _applianceFactory.GenerateAppliance(appliance1.ApplianceType, newLevel, coordinate);
            _applianceFactory.DestroyObj(appliance1);
            _applianceFactory.DestroyObj(appliance2);
            return newAppliance;
        }

        private GridPawn MergeProducers(Producer producer1, Producer producer2, int newLevel, Vector2Int coordinate)
        {
            var newProducer = _producerFactory.GenerateProducer(producer1.ProducerType, newLevel, coordinate);
            _producerFactory.DestroyObj(producer1);
            _producerFactory.DestroyObj(producer2);
            return newProducer;
        }
        
        private GridPawn HandleUnknownType(GridPawn pawn, Vector2Int coordinate)
        {
            Debug.LogWarning($"Unknown grid type: {pawn.Type} at {coordinate}");
            return null;
        }


    }
}
