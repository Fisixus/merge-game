using System.Collections.Generic;
using Core.GridPawns;
using Core.Helpers;
using Input;
using MVP.Models.Interface;
using MVP.Presenters.Handlers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVP.Presenters
{
    public class MergePresenter
    {
        private readonly IGridModel _gridModel;
        private readonly GridPawnFactoryHandler _gridPawnFactoryHandler;
        
        private GridPawn _activePawn;

        public MergePresenter(IGridModel gridModel, GridPawnFactoryHandler gridPawnFactoryHandler)
        {
            _gridModel = gridModel;
            _gridPawnFactoryHandler = gridPawnFactoryHandler;
            
            UserInput.OnGridPawnSingleTouched += OnTouched;
            UserInput.OnGridPawnDoubleTouched += OnDoubleTouched;
            UserInput.OnGridPawnReleased += OnReleased;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        private void OnSceneUnloaded(Scene scene)
        {
            Dispose();
        }

        private void Dispose()
        {
            // Unsubscribe from static and instance events
            UserInput.OnGridPawnSingleTouched -= OnTouched;
            UserInput.OnGridPawnDoubleTouched -= OnDoubleTouched;
            UserInput.OnGridPawnReleased -= OnReleased;

            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
        
        private void OnTouched(GridPawn touchedGridPawn)
        {
            _activePawn = touchedGridPawn;
            _activePawn.SetSortingOrder(1000); //TODO:
        }
        private void OnDoubleTouched()
        {
            if (_activePawn is Producer producer)
            {
                // Get the closest isEmpty coordinate from _gridModel.Grid
                var emptyCoord = GridPositionHelper.FindClosestEmptyCoordinate(_activePawn.Coordinate, _gridModel.Grid);
                // Grid is full
                if (emptyCoord == null)
                {
                    Debug.Log("FULL!");
                    return;
                }
                int level = producer.GetApplianceLevelToProduce();
                var appliance = _gridPawnFactoryHandler.GenerateAppliance(producer.GeneratedApplianceType, level, emptyCoord.Value);
                _gridModel.UpdateGridPawns(new List<GridPawn>{appliance}, _activePawn.Coordinate, true);
                producer.ReduceCapacity();
            }
        }
        private void OnReleased()
        {
            
        }
    }
}
