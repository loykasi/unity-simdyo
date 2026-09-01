using System;
using UnityEngine;

public class ResizeTool : BaseTool
{
    public override ToolType Type => ToolType.Resize;

    private ResizeBox _resizeBox = new();
    private ResizeCircle _circleHandler = new();
    private ResizePolygon _polygonHandler = new();

    private IResize _handler;

    private UIToolManager _uiToolManager;
    private EntityGroup _selectionGroup => ObjectManager.Instance.SelectionGroup;

    public override void Enable()
    {
        base.Enable();

        if (_uiToolManager == null)
        {
            _uiToolManager = UIManager.Instance.Get<UIToolManager>();
        }

        if (_selectionGroup.Count == 1)
        {
            ShowResizeUI(_selectionGroup.Entities[0]);
        }

        ObjectManager.Instance.OnObjectSelected += OnObjectSelected;
        ObjectManager.Instance.OnObjectDeselected += OnObjectDeselected;
    }

    public override void Disable()
    {
        base.Disable();

        _handler = null;
        _uiToolManager.ResizeBound.gameObject.SetActive(false);

        ObjectManager.Instance.OnObjectSelected -= OnObjectSelected;
        ObjectManager.Instance.OnObjectDeselected -= OnObjectDeselected;

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        _handler?.UpdateBound();
    }

    private void ShowResizeUI(SceneEntity entity)
    {
        _uiToolManager.ResizeBound.rotation = Quaternion.identity;

        switch (entity)
        {
            case BoxEntity boxEntity:
                _uiToolManager.ResizeBound.gameObject.SetActive(true);
                _handler = _resizeBox;
                _handler.Init(boxEntity, _uiToolManager.ResizeBound);
                break;
            case CircleEntity circleEntity:
                _uiToolManager.ResizeBound.gameObject.SetActive(true);
                _handler = _circleHandler;
                _handler.Init(circleEntity, _uiToolManager.ResizeBound);
                break;
            case PolygonEntity polygonEntity:
                _uiToolManager.ResizeBound.gameObject.SetActive(true);
                _handler = _polygonHandler;
                _handler.Init(polygonEntity, _uiToolManager.ResizeBound);
                break;
        }

        KDebug.Log($"[Resize Tool] Handle {entity.EntityType}");
    }

    private void OnObjectSelected(SceneEntity entity)
    {
        if (_selectionGroup.Count == 1)
        {
            ShowResizeUI(entity);
            return;
        }

        _handler = null;
        _uiToolManager.ResizeBound.gameObject.SetActive(false);
    }

    private void OnObjectDeselected()
    {
        _handler = null;
        _uiToolManager.ResizeBound.gameObject.SetActive(false);
    }

    public void BeginResize(BoundsHandleDirection direction)
    {
        _handler.BeginResize(direction);
    }

    public void Resize(BoundsHandleDirection direction, Vector3 mousePosition)
    {
        _handler.Resize(direction, mousePosition);
        Physics2D.SyncTransforms();
    }
}