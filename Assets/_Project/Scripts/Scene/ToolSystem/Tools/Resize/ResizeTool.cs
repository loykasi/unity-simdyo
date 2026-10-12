using System;
using UnityEngine;

public class ResizeTool : BaseTool
{
    public override ToolType Type => ToolType.Resize;

    private readonly ResizeBox _resizeBox = new();
    private readonly ResizeCircle _circleHandler = new();
    private readonly ResizePolygon _polygonHandler = new();

    private IResize _handler;
    private UIToolManager _uiToolManager;
    private EntityGroup SelectionGroup => ObjectManager.Instance.SelectionGroup;

    public override void Enable()
    {
        if (_uiToolManager == null)
        {
            _uiToolManager = UIManager.Instance.Get<UIToolManager>();
        }

        if (SelectionGroup.Count == 1)
        {
            ShowResizeUI(SelectionGroup.Entities[0]);
        }

        ObjectManager.Instance.OnObjectSelected += OnObjectSelected;
        ObjectManager.Instance.OnObjectDeselected += OnObjectDeselected;
    }

    public override void Disable()
    {
        _handler = null;
        ObjectManager.Instance.OnObjectSelected -= OnObjectSelected;
        ObjectManager.Instance.OnObjectDeselected -= OnObjectDeselected;

        if (_uiToolManager)
        {
            _uiToolManager.ResizeBound.gameObject.SetActive(false);
        }
    }

    public override void OnUpdate()
    {
        _handler?.UpdateBound();
    }

    private void ShowResizeUI(SceneEntity entity)
    {
        switch (entity)
        {
            case BoxEntity:
                _handler = _resizeBox;
                break;
            case CircleEntity:
                _handler = _circleHandler;
                break;
            case PolygonEntity:
                _handler = _polygonHandler;
                break;
            default:
                _handler = null;
                break;
        }

        if (_handler == null)
        {
            return;
        }

        _handler.Init(entity, _uiToolManager.ResizeBound);

        if (_uiToolManager)
        {
            _uiToolManager.ResizeBound.rotation = Quaternion.identity;
            _uiToolManager.ResizeBound.gameObject.SetActive(true);
        }

        KDebug.Log($"[Resize Tool] Handle {entity.EntityType}");
    }

    private void OnObjectSelected(SceneEntity entity)
    {
        if (SelectionGroup.Count == 1)
        {
            ShowResizeUI(entity);
            return;
        }

        _handler = null;
        if (_uiToolManager)
        {
            _uiToolManager.ResizeBound.gameObject.SetActive(false);   
        }
    }

    private void OnObjectDeselected()
    {
        _handler = null;
        if (_uiToolManager)
        {
            _uiToolManager.ResizeBound.gameObject.SetActive(false);
        }
    }

    public void BeginResize(BoundsHandleDirection direction)
    {
        _handler?.BeginResize(direction);
    }

    public void Resize(BoundsHandleDirection direction, Vector3 mousePosition)
    {
        if (_handler == null)
        {
            return;
        }
        _handler.Resize(direction, mousePosition);
        Physics2D.SyncTransforms();
    }
}