using UnityEngine;

public class MasterTool
{
    private ITool _tool;

    private Vector2 _pointerDownPosition;
    private bool _isPanning;
    private bool _isDragged;

    private readonly InteractionPan _pan = new();
    private readonly InteractionZoom _zoom = new();

    private const float DragThresholdSqr = 0.04f;

    public void Subscribe()
    {
        InputManager.Instance.ClickEvent += OnLeftDown;
        InputManager.Instance.ClickReleasedEvent += OnLeftUp;
        InputManager.Instance.RightClickEvent += OnRightDown;
        InputManager.Instance.RightClickReleasedEvent += OnRightUp;
        InputManager.Instance.MouseMoveEvent += OnMove;
        InputManager.Instance.ScrollEvent += OnScroll;
    }

    public void Unsubscribe()
    {
        if (InputManager.Instance)
        {
            InputManager.Instance.ClickEvent -= OnLeftDown;
            InputManager.Instance.ClickReleasedEvent -= OnLeftUp;
            InputManager.Instance.RightClickEvent -= OnRightDown;
            InputManager.Instance.RightClickReleasedEvent -= OnRightUp;
            InputManager.Instance.MouseMoveEvent -= OnMove;
            InputManager.Instance.ScrollEvent -= OnScroll;   
        }
    }

    public void SetTool(ITool tool)
    {
        _tool = tool;
    }

    public void Update()
    {
        _zoom.Update();
        _tool?.OnUpdate();
    }

    private void OnLeftDown()
    {
        if (ScreenInteractionUtils.IsOverUI())
        {
            return;
        }

        Vector2 pos = InputManager.Instance.MousePosition;
        _pointerDownPosition = pos;
        _isDragged = false;

        bool isConsumed = _tool?.OnPointerLeftDown(pos) ?? false;
        if (!isConsumed)
        {
            _pan.Start();
            _isPanning = true;
        }
    }

    private void OnLeftUp()
    {
        if (_isPanning)
        {
            _pan.Stop();
            _isPanning = false;
        }
        
        Vector2 pos = InputManager.Instance.MousePosition;
        _tool?.OnPointerLeftUp(pos);

        if (_tool != null && _tool.IsAllowSelectOnClick && !_isDragged)
        {
            ObjectManager.Instance.Select(_pointerDownPosition);
        }
    }

    private void OnRightDown()
    {
        if (ScreenInteractionUtils.IsOverUI())
        {
            return;
        }

        Vector2 pos = InputManager.Instance.MousePosition;
        _pointerDownPosition = pos;
        _isDragged = false;

        bool isConsumed = _tool?.OnPointerRightDown(pos) ?? false;
        if (!isConsumed)
        {
            _pan.Start();
            _isPanning = true;
        }
    }

    private void OnRightUp()
    {
        if (_isPanning)
        {
            _pan.Stop();
            _isPanning = false;
        }

        Vector2 pos = InputManager.Instance.MousePosition;
        _tool?.OnPointerRightUp(pos);
        
        if (!_isDragged)
        {
            ContextMenuManager.Instance.Open(pos);
        }
    }

    private void OnMove(Vector2 position)
    {
        if ((position - _pointerDownPosition).sqrMagnitude > DragThresholdSqr)
        {
            _isDragged = true;
        }

        if (_isPanning)
        {
            _pan.Pan(position);
            return;
        }
        _tool?.OnPointerMove(position);
    }

    private void OnScroll(float delta)
    {
        if (ScreenInteractionUtils.IsOverUI()) return;
        if (_tool?.OnScroll(delta) ?? false) return;
        _zoom.OnScroll(delta);
    }
}