using System;
using UnityEngine;

public class ResizeController : Singleton<ResizeController>
{
    [SerializeField] private RectTransform _bound;

    private ResizeBox _resizeBox = new();
    private CircleResizeHandler _circleHandler = new();

    private IResize _handler;

    private bool _enabled;

    public void Enable()
    {
        _enabled = true;
        var entity = ObjectManager.Instance.SelectedObject;
        OnObjectSelected(entity);
    }

    public void Disable()
    {
        _enabled = false;
        _bound.gameObject.SetActive(false);
    }

    private void Start()
    {
        ObjectManager.Instance.OnObjectSelected += OnObjectSelected;
        ObjectManager.Instance.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnObjectDeselected()
    {
        _bound.gameObject.SetActive(false);
    }

    private void OnObjectSelected(SceneEntity entity)
    {
        if (!_enabled)
        {
            return;
        }
        
        if (entity is BoxEntity boxEntity)
        {
            _bound.gameObject.SetActive(true);

            _handler = _resizeBox;
            _handler.Init(boxEntity, _bound);
            return;
        }

        if (entity is CircleEntity circleEntity)
        {
            _bound.gameObject.SetActive(true);

            _handler = _circleHandler;
            _handler.Init(circleEntity, _bound);
            return;
        }
    }

    private void Update()
    {
        if (!_enabled)
        {
            return;
        }

        _handler?.UpdateBound();
    }

    public void BeginResize(BoundsHandleDirection direction)
    {
        _handler.BeginResize(direction);
    }

    public void Resize(BoundsHandleDirection direction, Vector3 mousePosition)
    {
        _handler.Resize(direction, mousePosition);
    }
}