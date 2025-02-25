
using System.Linq;
using Core.Factories.Interface;
using Core.GridPawns;
using Core.Helpers;
using Core.Inventories;
using Input;
using MVP.Models.Interface;
using MVP.Presenters.Handlers;
using MVP.Views.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVP.Presenters
{
    public class InventoryPresenter
    {
        private readonly IInventoryModel _inventoryModel;
        private readonly IInventoryView _inventoryView;
        private readonly IInventoryPawnUIFactory _inventoryPawnUIFactory;
        private readonly IGridModel _gridModel;
        private readonly GridPawnFactoryHandler _gridPawnFactoryHandler;
        private readonly TaskPresenter _taskPresenter;

        private GridPawn _activePawn;

        public InventoryPresenter(IInventoryModel inventoryModel, IInventoryView inventoryView, IInventoryPawnUIFactory inventoryPawnUIFactory,
            IGridModel gridModel, GridPawnFactoryHandler gridPawnFactoryHandler, TaskPresenter taskPresenter)
        {
            _inventoryModel = inventoryModel;
            _inventoryView = inventoryView;
            _inventoryPawnUIFactory = inventoryPawnUIFactory;
            _gridModel = gridModel;
            _gridPawnFactoryHandler = gridPawnFactoryHandler;
            _taskPresenter = taskPresenter;
            
            UserInput.OnGridPawnSingleTouched += OnTouched;
            UserInput.OnGridPawnReleased += OnReleased;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            
            _inventoryView.SubscribeInventoryButton(this);
            //TODO:LOAD Inventory
            
        }
        private void OnSceneUnloaded(Scene scene)
        {
            Dispose();
        }
        private void Dispose()
        {
            // Unsubscribe from static and instance events
            UserInput.OnGridPawnSingleTouched -= OnTouched;
            UserInput.OnGridPawnReleased -= OnReleased;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        public void OnPawnRequestedFromInventory(InventoryPawn inventoryPawn)
        {
            var matchingUI = _inventoryView.InventoryPawnUIs
                .FirstOrDefault(ui => ui.InventoryPawn.ID == inventoryPawn.ID);

            if (matchingUI != null)
            {
                var randomCoordinate = GridPositionHelper.FindRandomEmptyCoordinate(_gridModel.Grid);
                if (randomCoordinate == null)
                {
                    //No space in grid
                    matchingUI.Shake();
                    return;
                }
                
                //TODO Play particle effect maybe
                _inventoryPawnUIFactory.DestroyObj(matchingUI);
                _inventoryView.InventoryPawnUIs.Remove(matchingUI);
                //Save Inventory
                //TODO:_inventoryModel.AddPawn(newInventoryPawnUI.InventoryPawn);
                
                var newPawn = _gridPawnFactoryHandler.CreateGridPawn(inventoryPawn.Type, inventoryPawn.Level, randomCoordinate.Value);
                _gridModel.UpdateGridPawn(newPawn, false, null, true, 0.2f);
                _taskPresenter.UpdateTasks();
            }
            else
            {
                Debug.LogWarning($"No InventoryPawnUI found with ID: {inventoryPawn.ID}");
            }        
        }
        
        public void OnInventoryRequested()
        {
            if (_activePawn == null)
            {
                _inventoryView.OpenInventoryPanel();
            }
            
        }

        private void OnTouched(GridPawn touchedGridPawn)
        {
            _activePawn = touchedGridPawn;
        }

        private void OnReleased()
        {
            if(_activePawn != null && _inventoryView.InventoryButton.IsEntered)
            {
                //TODO:Play particle effect 
                
                //Add Inventory
                var newInventoryPawnUI = _inventoryPawnUIFactory.CreateObj();
                newInventoryPawnUI.FillData(_activePawn);
                newInventoryPawnUI.SubscribeToInventoryPawnUIClick(this);
                _inventoryView.InventoryPawnUIs.Add(newInventoryPawnUI);
                //Save Inventory
                //TODO:_inventoryModel.AddPawn(newInventoryPawnUI.InventoryPawn);
                
                //Destroy from grid
                _gridPawnFactoryHandler.DestroyPawn(_activePawn);
                _gridModel.UpdateGridPawn(_activePawn, true);
                _activePawn.PawnEffect.SetFocus(false);
                _taskPresenter.UpdateTasks();

               
                
                _activePawn = null;
            }
        }
    }
}
