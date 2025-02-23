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
        public static event Action<GridPawn> OnGridPawnDoubleTouched;
        public static event Action<GridPawn> OnGridPawnReleased;
        private IA_User _iaUser;
        
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

        private void GetPawn()
        {
            if (IsPointerOverUIObject() || !_isInputOn)
                return;
            var hit = Physics2D.Raycast(_cam.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector2.zero);
            if (hit && hit.transform.TryGetComponent<GridPawn>(out var gridPawn))
            {
                if(_activePawn != null) _activePawn.GetComponent<GridPawnEffect>().SetFocus(0f);
                _activePawn = gridPawn;
            }
            else
            {
                _activePawn = null;
            } 
        }
        
        private void OnSingleTouch(InputAction.CallbackContext context)
        {
            GetPawn();
            if(_activePawn == null) return;
            _activePawn.GetComponent<GridPawnEffect>().SetFocus(1f);
            _isDragging = true;
            // Check if input is from touchscreen or mouse
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                _lastPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            }
            else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                _lastPosition = Mouse.current.position.ReadValue();
            }
            Debug.Log("Drag Started at: " + _lastPosition);
        }

        private void OnRelease(InputAction.CallbackContext context)
        {
            if(_activePawn == null) return;

            _isDragging = false;
            Debug.Log("Drag Stopped.");
        }

        private void OnDrag(InputAction.CallbackContext context)
        {
            if(_activePawn == null) return;
            if (!_isDragging) return;

            Vector2 currentPosition = context.ReadValue<Vector2>();
            Vector2 delta = currentPosition - _lastPosition;

            Debug.Log($"Dragging... Delta: {delta}");
            _lastPosition = currentPosition;
        }

        private void OnDoubleTouch(InputAction.CallbackContext context)
        {
            Debug.Log("Double Tap Detected!");
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
