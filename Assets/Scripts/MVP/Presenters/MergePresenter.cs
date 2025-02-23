using System.Collections.Generic;
using Core.GridEffects;
using Core.GridPawns;
using Core.GridPawns.Effect;
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
        private readonly DisappearEffectHandler _disappearEffectHandler;
        
        private GridPawn _activePawn;

        public MergePresenter(IGridModel gridModel, GridPawnFactoryHandler gridPawnFactoryHandler, DisappearEffectHandler disappearEffectHandler)
        {
            _gridModel = gridModel;
            _gridPawnFactoryHandler = gridPawnFactoryHandler;
            _disappearEffectHandler = disappearEffectHandler;
            
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
        
        private void OnReleased()
        {
            if(_activePawn == null) return;
            _activePawn.PawnEffect.SetFocus(1);

            var closestCoordinate = GridPositionHelper.FindClosestCoordinateAfterRelease(_activePawn.transform.position);
            var oldPos = GridPositionHelper.GetWorldPositionFromCoordinate(_activePawn.Coordinate);
            
            if (closestCoordinate == null)
            {
                _activePawn.SetWorldPosition(oldPos, true, 0.3f);
            }
            else
            {
                var pawn = _gridModel.Grid[closestCoordinate.Value.x, closestCoordinate.Value.y];
                var pawnPos = GridPositionHelper.GetWorldPositionFromCoordinate(pawn.Coordinate);
                
                if (_activePawn.Level == pawn.Level && _activePawn.Type.Equals(pawn.Type))
                {

                }
                else
                {
                    var firstCoord = _activePawn.Coordinate;
                    var secondCoord = pawn.Coordinate;
                    GridItemModifierHelper.SwapItems(_gridModel.Grid, firstCoord.x, firstCoord.y, secondCoord.x, secondCoord.y);

                    pawn.SetWorldPosition(oldPos, true, 0.3f);
                    _activePawn.SetWorldPosition(pawnPos, true, 0.3f);
                }
            }
        }
        
        private void OnDoubleTouched()
        {
            if (!(_activePawn is Producer producer)) return;

            TryProduceAppliance(producer);
        }

        private void TryProduceAppliance(Producer producer)
        {
            var emptyCoord = FindClosestEmptyCoordinate(producer.Coordinate);

            if (emptyCoord == null) return; // Grid is full

            ProduceAppliance(producer, emptyCoord.Value);
            HandleProducerCapacity(producer);
        }

        private Vector2Int? FindClosestEmptyCoordinate(Vector2Int origin)
        {
            return GridPositionHelper.FindClosestEmptyCoordinate(origin, _gridModel.Grid);
        }

        private void ProduceAppliance(Producer producer, Vector2Int emptyCoord)
        {
            int level = producer.GetApplianceLevelToProduce();
            var appliance = _gridPawnFactoryHandler.GenerateAppliance(producer.GeneratedApplianceType, level, emptyCoord);
    
            _gridModel.UpdateGridPawns(new List<GridPawn> { appliance }, _activePawn.Coordinate, true, 0.3f);
            producer.ReduceCapacity();
        }

        private void HandleProducerCapacity(Producer producer)
        {
            if (producer.Capacity > 0) return;

            var newPosition = GridPositionHelper.FindRandomEmptyCoordinate(_gridModel.Grid) ?? producer.Coordinate;
            ReplaceProducer(producer, newPosition);
        }
        
        private void ReplaceProducer(Producer producer, Vector2Int newPosition)
        {
            _activePawn.PawnEffect.SetFocus(0);
            _activePawn = null;//TODO:
            
            _disappearEffectHandler.PlayDisappearEffect(producer.transform.position, ColorType.White).Forget();
            
            var newProducer = _gridPawnFactoryHandler.RecycleProducer(producer, newPosition);
            _gridModel.UpdateGridPawns(new List<GridPawn> { newProducer }, 
                new Vector2Int(newPosition.x, newPosition.y - _gridModel.ColumnCount), true, 0.7f);
        }


    }
}
