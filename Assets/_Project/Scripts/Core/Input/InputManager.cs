using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>, GameInput.IEditorActions
{
    public DelegateList ClickEvent = new();
    public DelegateList ClickReleasedEvent = new();
    public DelegateList RightClickEvent = new();
    public DelegateList RightClickReleasedEvent = new();
    public DelegateList<float> ScrollEvent = new();
    public DelegateList<Vector2> MouseMoveEvent = new();

    // public Vector2 MousePosition => _gameInput.Editor.Point.ReadValue<Vector2>();
    public Vector2 MousePosition => Mouse.current.position.ReadValue();
    public bool IsMultipleSelect;

    private GameInput _gameInput;

    protected override void Awake()
    {
        base.Awake();
        _gameInput = new GameInput();
        _gameInput.Editor.SetCallbacks(this);
        EnableEditorActions();
    }

    public void EnableEditorActions()
    {
        _gameInput.Editor.Enable();
    }

    public void DisableEditorActions()
    {
        _gameInput.Editor.Disable();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                ClickEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                ClickReleasedEvent.Invoke();
                break;
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                RightClickEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                RightClickReleasedEvent.Invoke();
                break;
        }
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ScrollEvent.Invoke(context.ReadValue<Vector2>().y);
        }
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MouseMoveEvent.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnMultipleSelect(InputAction.CallbackContext context)
    {
        IsMultipleSelect = context.phase.IsInProgress();
    }
}