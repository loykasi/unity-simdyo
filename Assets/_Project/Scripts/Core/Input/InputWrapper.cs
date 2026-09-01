using System;
using UnityEngine.InputSystem;

public class InputWrapper<T> where T : struct
{
    protected readonly InputAction _action;

    public T Value;
    public Action<InputWrapper<T>> OnAction;
    public bool IsInProgress { get; private set; }
    public bool IsReleased => !IsInProgress;

    public InputWrapper(InputAction action)
    {
        _action = action;

        _action.started += Invoke;
        _action.performed += Invoke;
        _action.canceled += Invoke;
    }

    public void Unbind()
    {
        _action.started -= Invoke;
        _action.performed -= Invoke;
        _action.canceled -= Invoke;
    }

    public void Invoke(InputAction.CallbackContext context)
    {
        IsInProgress = context.IsInProgress();
        Value = context.ReadValue<T>();
        
        OnAction?.Invoke(this);
    }
}