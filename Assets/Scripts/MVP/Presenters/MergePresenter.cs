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
        private readonly MergeGlowEffectHandler _mergeGlowEffectHandler;
        
        private GridPawn _activePawn;
        private int _activeSortingOrder;

        public MergePresenter(IGridModel gridModel, GridPawnFactoryHandler gridPawnFactoryHandler, 
            DisappearEffectHandler disappearEffectHandler, MergeGlowEffectHandler mergeGlowEffectHandler)
        {
            _gridModel = gridModel;
            _gridPawnFactoryHandler = gridPawnFactoryHandler;
            _disappearEffectHandler = disappearEffectHandler;
            _mergeGlowEffectHandler = mergeGlowEffectHandler;
            
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
            if (_activePawn != null)
            {
                _activePawn.PawnEffect.SetFocus(false);
            }
    
            _activePawn = touchedGridPawn;
            _activeSortingOrder = _activePawn.SpriteRenderer.sortingOrder;
            _activePawn.SetSortingOrder(1000, "UI"); // TODO: Ensure sorting logic is correct

            if (ShouldDestroy(_activePawn))
            {
                HandlePawnDestruction(_activePawn);
            }
        }
        private bool ShouldDestroy(GridPawn pawn)
        {
            return pawn is Appliance && pawn.Level == pawn.MaxLevel;
        }

        private void HandlePawnDestruction(GridPawn pawn)
        {
            _disappearEffectHandler.PlayDisappearEffect(pawn.transform.position, ColorType.White).Forget();
            _gridPawnFactoryHandler.DestroyPawn(pawn);
            _gridModel.UpdateGridPawns(new List<GridPawn> { pawn }, null, false);

            pawn.PawnEffect.SetFocus(false);
            _activePawn = null;
        }
        
        private void OnReleased()
        {
            if (_activePawn == null) return;
            _activePawn.SetSortingOrder(_activeSortingOrder, "Pawns");
            _activePawn.PawnEffect.SetFocus(true);

            var closestCoordinate = GridPositionHelper.FindClosestCoordinateAfterRelease(_activePawn.transform.position);
            var oldPos = GridPositionHelper.GetWorldPositionFromCoordinate(_activePawn.Coordinate);

            if (closestCoordinate == null)
            {
                ResetPawnPosition(oldPos);
                return;
            }

            var targetPawn = _gridModel.Grid[closestCoordinate.Value.x, closestCoordinate.Value.y];
    
            if (targetPawn.Equals(_activePawn)) return;

            HandlePawnInteraction(targetPawn, oldPos);
        }

        private void ResetPawnPosition(Vector3 position)
        {
            _activePawn.SetWorldPosition(position, true, 0.3f);
        }

        private void HandlePawnInteraction(GridPawn targetPawn, Vector3 oldPos)
        {
            var targetPos = GridPositionHelper.GetWorldPositionFromCoordinate(targetPawn.Coordinate);

            if (CanMergeWith(targetPawn))
            {
                MergePawns(targetPawn);
            }
            else
            {
                SwapPawns(targetPawn, oldPos, targetPos);
            }
        }

        private bool CanMergeWith(GridPawn targetPawn)
        {
            return _activePawn.Level < _activePawn.MaxLevel &&
                   _activePawn.Level == targetPawn.Level &&
                   _activePawn.Type.Equals(targetPawn.Type);
        }

        private void MergePawns(GridPawn targetPawn)
        {
            var newPawn = _gridPawnFactoryHandler.MergePawns(_activePawn, targetPawn);
            newPawn.PawnEffect.SetFocus(true);
            
            _gridModel.UpdateGridPawns(new List<GridPawn> { newPawn }, null, false);
            _mergeGlowEffectHandler.PlayMergeGlowEffect(newPawn.transform.position, ColorType.Blue).Forget();
            
            _activePawn.PawnEffect.SetFocus(false);
            _activePawn = newPawn;
        }

        private void SwapPawns(GridPawn targetPawn, Vector3 oldPos, Vector3 targetPos)
        {
            _gridModel.SwapGridItems(_activePawn, targetPawn);

            targetPawn.SetWorldPosition(oldPos, true, 0.3f);
            _activePawn.SetWorldPosition(targetPos, true, 0.3f);
        }

        private void OnDoubleTouched()
        {
            if (!(_activePawn is Producer producer)) return;

            TryProduceAppliance(producer);
        }

        private void TryProduceAppliance(Producer producer)
        {
            var emptyCoord = GridPositionHelper.FindClosestEmptyCoordinate(producer.Coordinate, _gridModel.Grid);

            if (emptyCoord == null) return; // Grid is full

            ProduceAppliance(producer, emptyCoord.Value);
            HandleProducerCapacity(producer);
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
            _activePawn.PawnEffect.SetFocus(false);
            _activePawn = null;
            
            _disappearEffectHandler.PlayDisappearEffect(producer.transform.position, ColorType.White).Forget();
            
            var newProducer = _gridPawnFactoryHandler.RecycleProducer(producer, newPosition);
            _gridModel.UpdateGridPawns(new List<GridPawn> { newProducer }, 
                new Vector2Int(newPosition.x, newPosition.y - _gridModel.ColumnCount), true, 0.7f);
        }


    }
}
