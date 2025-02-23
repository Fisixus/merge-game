using Core.GridPawns;
using Input;
using MVP.Models.Interface;
using UnityEngine.SceneManagement;

namespace MVP.Presenters
{
    public class MergePresenter
    {
        private readonly IGridModel _gridModel;
        private GridPawn _activePawn;

        public MergePresenter(IGridModel gridModel)
        {
            _gridModel = gridModel;
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
            _activePawn.SetSortingOrder(1000);
        }
        private void OnDoubleTouched()
        {
            if (_activePawn is Producer producer)
            {
                
            }
        }
        private void OnReleased()
        {
            
        }
    }
}
