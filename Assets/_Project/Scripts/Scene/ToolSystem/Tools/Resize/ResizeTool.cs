using UnityEngine;

public class ResizeTool : PanTool
{
    public override ToolType Type => ToolType.Resize;

    [SerializeField] private RectTransform _bound;

    private ResizeBox _resizeBox = new();
    private ResizeCircle _circleHandler = new();
    private ResizePolygon _polygonHandler = new();

    private IResize _handler;

    private bool _enabled;

    private void Start()
    {
        ObjectManager.Instance.OnObjectSelected += OnObjectSelected;
        ObjectManager.Instance.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnObjectDeselected()
    {
        _bound.gameObject.SetActive(false);
        _handler = null;
    }

    public override void Enable()
    {
        _enabled = true;
        var entity = ObjectManager.Instance.SelectedObject;
        OnObjectSelected(entity);
    }

    public override void Disable()
    {
        _enabled = false;
        _bound.gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        if (!_enabled || _handler == null)
        {
            return;
        }

        _handler?.UpdateBound();
    }

    private void OnObjectSelected(SceneEntity entity)
    {
        if (!_enabled)
        {
            return;
        }

        _bound.rotation = Quaternion.identity;
        
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

        if (entity is PolygonEntity polygonEntity)
        {
            _bound.gameObject.SetActive(true);

            _handler = _polygonHandler;
            _handler.Init(polygonEntity, _bound);
            return;
        }
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