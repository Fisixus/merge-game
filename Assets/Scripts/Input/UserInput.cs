using System;
using System.Collections.Generic;
using Core.GridPawns;
using Core.GridPawns.Effect;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Input
{
    public class UserInput : MonoBehaviour
    {
        private Camera _cam;
        private EventSystem _eventSystem;
        
        private bool _isDragging = false;
        private Vector2 _lastPosition;
        private GridPawn _activePawn;

        private static bool _isInputOn = true;
        public static event Action<GridPawn> OnGridPawnSingleTouched;
        public static event Action OnGridPawnDoubleTouched;
        public static event Action OnGridPawnReleased;
        
        private IA_User _iaUser;
        private bool _isDoubleClickedSamePawn;
        
        private void Awake()
        {
            _cam = Camera.main;
            _eventSystem = EventSystem.current;
            _iaUser = new IA_User(); // Instantiate the input actions class
            _iaUser.Pawn.Enable(); // Enable the specific action map
            
            _iaUser.Pawn.SingleTouch.performed += OnSingleTouch;
            _iaUser.Pawn.DoubleTouch.performed += OnDoubleTouch;
            _iaUser.Pawn.Release.performed += OnRelease;
            _iaUser.Pawn.Drag.performed += OnDrag;
            //_iaUser.Match.Touch.performed += TouchItemNotifier; // Subscribe to the action
        }

        private void OnDisable()
        {
            _iaUser.Pawn.SingleTouch.performed -= OnSingleTouch;
            _iaUser.Pawn.DoubleTouch.performed -= OnDoubleTouch;
            _iaUser.Pawn.Release.performed -= OnRelease;
            _iaUser.Pawn.Drag.performed -= OnDrag;
        }

        private GridPawn GetPawnAtPointer()
        {
            if (IsPointerOverUIObject() || !_isInputOn)
                return null;

            var hit = Physics2D.Raycast(_cam.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector2.zero);
            return hit && hit.transform.TryGetComponent<GridPawn>(out var gridPawn) ? gridPawn : null;
        }

        private void SetActivePawn(GridPawn newPawn)
        {
            if (_activePawn != null)
                _activePawn.PawnEffect.SetFocus(false);

            _activePawn = newPawn;
        }
        
        private void OnSingleTouch(InputAction.CallbackContext context)
        {
            var newPawn = GetPawnAtPointer();
            _isDoubleClickedSamePawn = (newPawn != null && newPawn.Equals(_activePawn));

            SetActivePawn(newPawn);
            if (_activePawn == null) return;

            _activePawn.PawnEffect.SetFocus(true);
            _isDragging = true;
            OnGridPawnSingleTouched?.Invoke(_activePawn);

            //  Determine last touch position (Touchscreen or Mouse)
            if (Touchscreen.current?.primaryTouch.press.isPressed == true)
            {
                _lastPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            }
            else if (Mouse.current?.leftButton.isPressed == true)
            {
                _lastPosition = Mouse.current.position.ReadValue();
            }
        }


        private void OnRelease(InputAction.CallbackContext context)
        {
            if(_activePawn == null) return;

            _isDragging = false;
            OnGridPawnReleased?.Invoke();

            //Debug.Log("Drag Stopped.");
        }

        private void OnDrag(InputAction.CallbackContext context)
        {
            if (_activePawn == null || !_isDragging) return;
            _activePawn.PawnEffect.SetFocus(false);

            Vector2 currentPosition = context.ReadValue<Vector2>();

            var pawnPos = _activePawn.transform.position;
            Vector3 worldStart = _cam.ScreenToWorldPoint(new Vector3(_lastPosition.x, _lastPosition.y, pawnPos.z));
            Vector3 worldCurrent = _cam.ScreenToWorldPoint(new Vector3(currentPosition.x, currentPosition.y, pawnPos.z));

            Vector3 worldDelta = worldCurrent - worldStart;

            // Move the active pawn
            pawnPos += worldDelta;
            _activePawn.transform.position = pawnPos;

            _lastPosition = currentPosition;
            //Debug.Log($"Dragging... New Position: {_activePawn.transform.position}");
        }

        private void OnDoubleTouch(InputAction.CallbackContext context)
        {
            //Debug.Log("Double Tap Detected!");
            if(!_isDoubleClickedSamePawn) return;
            OnGridPawnDoubleTouched?.Invoke();
        }
        
        public static void SetInputState(bool isInputOn)
        {
            _isInputOn = isInputOn;
        }

        private bool IsPointerOverUIObject()
        {
            // Create PointerEventData for the current event system
            PointerEventData eventData = new PointerEventData(_eventSystem);

#if UNITY_EDITOR || UNITY_STANDALONE
            // Use mouse position for PC builds and the Unity editor
            eventData.position = UnityEngine.Input.mousePosition;
#else
        // Use touch position for mobile devices
        if (UnityEngine.Input.touchCount > 0)
            eventData.position = UnityEngine.Input.GetTouch(0).position;
        else
            return false;
#endif

            // Perform a raycast and check if any UI elements were hit
            List<RaycastResult> results = new List<RaycastResult>();
            _eventSystem.RaycastAll(eventData, results);

            // Return true if any UI elements were hit, false otherwise
            return results.Count > 0;
        }
        
        
    }
}
