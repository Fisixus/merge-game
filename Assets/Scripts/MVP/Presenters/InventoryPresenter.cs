
using Core.Factories.Interface;
using Core.GridPawns;
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
                //Destroy from grid
                _gridPawnFactoryHandler.DestroyPawn(_activePawn);
                _gridModel.UpdateGridPawn(_activePawn, true);
                _activePawn.PawnEffect.SetFocus(false);
                _taskPresenter.UpdateTasks();

                //Add Inventory
                var newInventoryPawnUI = _inventoryPawnUIFactory.CreateObj();
                newInventoryPawnUI.FillData(_activePawn);
                _inventoryView.InventoryPawnUis.Add(newInventoryPawnUI);
                //Save Inventory
                _inventoryModel.AddPawn(newInventoryPawnUI.InventoryPawn);
                
                _activePawn = null;
            }
        }
    }
}
