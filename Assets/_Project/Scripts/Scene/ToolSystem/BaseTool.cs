using System;
using UnityEngine;

public abstract class BaseTool : ITool
{
    public abstract ToolType Type { get; }

    protected InteractionPan _pan = new();
    protected InteractionZoom _zoom = new();
    protected InteractionSelect _select = new();
    protected InteractionContextMenu _contextMenu = new();

    private Action _clickEvent;
    private Action _clickReleasedEvent;
    private Action _rightClickEvent;
    private Action _rightClickReleasedEvent;
    private Action<Vector2> _mouseMoveEvent;

    public BaseTool()
    {
        _clickEvent = OnClick;
        _clickReleasedEvent = OnClickReleased;
        _rightClickEvent = OnRightClick;
        _rightClickReleasedEvent = OnRightClickReleased;
        _mouseMoveEvent = OnPointMove;
    }

    public virtual void Enable()
    {
        InputManager.Instance.ClickEvent += _clickEvent;
        InputManager.Instance.ClickReleasedEvent += _clickReleasedEvent;
        InputManager.Instance.RightClickEvent += _rightClickEvent;
        InputManager.Instance.RightClickReleasedEvent += _rightClickReleasedEvent;
        InputManager.Instance.MouseMoveEvent += _mouseMoveEvent;

        _zoom.Enable();
        _select.Enable();
        _contextMenu.Enable();
    }

    public virtual void Disable()
    {
        InputManager.Instance.ClickEvent -= _clickEvent;
        InputManager.Instance.ClickReleasedEvent -= _clickReleasedEvent;
        InputManager.Instance.RightClickEvent -= _rightClickEvent;
        InputManager.Instance.RightClickReleasedEvent -= _rightClickReleasedEvent;
        InputManager.Instance.MouseMoveEvent -= _mouseMoveEvent;

        _zoom.Disable();
        _select.Disable();
        _contextMenu.Disable();
    }

    protected virtual void OnClick()
    {
        
    }

    protected virtual void OnClickReleased()
    {
        
    }

    protected virtual void OnRightClick()
    {
        _pan.Start();
    }

    protected virtual void OnRightClickReleased()
    {
        _pan.Stop();
    }

    protected virtual void OnPointMove(Vector2 value)
    {
        _pan.Pan(value);
    }

    public virtual void OnUpdate()
    {
        _zoom.Update();
    }
}