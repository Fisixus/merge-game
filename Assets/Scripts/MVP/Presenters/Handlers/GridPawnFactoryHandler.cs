using System;
using System.Collections.Generic;
using Core.Factories.Interface;
using Core.GridPawns;
using Core.GridPawns.Enum;
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
                    if (gridPawn != null)
                    {
                        gridPawns.Add(gridPawn);
                    }
                }
            }
        }
        
        public void DestroyAllGridPawns()
        {
            _applianceFactory.DestroyAllAppliances();
            _producerFactory.DestroyAllProducers();
        }
        
        private GridPawn CreateGridPawn(Enum pawnType, int level, Vector2Int coordinate)
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
    }
}
