using System.Collections.Generic;
using Core.GridEffects;
using Core.GridPawns;
using Core.GridPawns.Effect;
using Core.Helpers;
using Input;
using JetBrains.Annotations;
using MVP.Models.Interface;
using MVP.Presenters.Handlers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVP.Presenters
{
    public class MergePresenter
    {
        private readonly TaskPresenter _taskPresenter;
        private readonly IGridModel _gridModel;
        private readonly GridPawnFactoryHandler _gridPawnFactoryHandler;
        private readonly DisappearEffectHandler _disappearEffectHandler;
        private readonly MergeGlowEffectHandler _mergeGlowEffectHandler;
        
        private GridPawn _activePawn;
        private int _activeSortingOrder;

        public MergePresenter(TaskPresenter taskPresenter, IGridModel gridModel, GridPawnFactoryHandler gridPawnFactoryHandler, 
            DisappearEffectHandler disappearEffectHandler, MergeGlowEffectHandler mergeGlowEffectHandler)
        {
            _taskPresenter = taskPresenter;
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
            _activePawn.SetSortingOrder(1000, "UI");

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
            _gridModel.UpdateGridPawn(pawn, true);

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

            HandlePawnInteraction(closestCoordinate.Value, oldPos);
        }

        private void HandlePawnInteraction(Vector2Int targetCoord, Vector3 oldPos)
        {
            var snappingWorldPosition = GridPositionHelper.GetWorldPositionFromCoordinate(targetCoord);
            var targetPawn = _gridModel.Grid[targetCoord.x, targetCoord.y];

            if (targetPawn == null)
            {
                SwapPawns(targetCoord, oldPos, snappingWorldPosition);
            }
            else if (!targetPawn.Equals(_activePawn))
            {
                if (CanMergeWith(targetPawn))
                {
                    MergePawns(targetPawn);
                }
                else
                {
                    SwapPawns(targetCoord, oldPos, snappingWorldPosition);
                }
            }
        }

        private void SwapPawns(Vector2Int targetCoord, Vector3 oldPos, Vector3 targetPos)
        {
            if (_activePawn == null) return;

            var targetPawn = _gridModel.Grid[targetCoord.x, targetCoord.y];

            targetPawn?.SetWorldPosition(oldPos, true, 0.3f);
            _activePawn.SetWorldPosition(targetPos, true, 0.3f);
            
            _gridModel.SwapGridItems(_activePawn, targetCoord);
        }

        private void ResetPawnPosition(Vector3 position)
        {
            _activePawn.SetWorldPosition(position, true, 0.3f);
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
            
            _gridModel.UpdateGridPawn(_activePawn, true);
            _gridModel.UpdateGridPawn(targetPawn, true);
            _gridModel.UpdateGridPawn(newPawn, false, null, false);

            _mergeGlowEffectHandler.PlayMergeGlowEffect(newPawn.transform.position, ColorType.Blue).Forget();
            
            _activePawn.PawnEffect.SetFocus(false);
            _activePawn = newPawn;

            _taskPresenter.UpdateTasks();
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
    
            _gridModel.UpdateGridPawn(appliance, false, _activePawn.Coordinate, true, 0.3f);
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
            _gridModel.UpdateGridPawn(producer, true);
            _gridModel.UpdateGridPawn(newProducer, false,
                new Vector2Int(newPosition.x, newPosition.y - _gridModel.ColumnCount), true, 0.7f);
        }


    }
}
