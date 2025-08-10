using UnityEngine;

public class GridController : Singleton<GridController>
{
    public bool GridEnabled { get; set; }
    public bool SnapEnabled { get; set; }

    [SerializeField] private GridOverlay _gridOverlay;
    [SerializeField] private Material _gridMaterial;

    [SerializeField] private int _gridBase;
    [SerializeField] private float _maxSize;

    private float _size = 1.0f;
    private float _subSize = 0.25f;
    private Vector2 _sizeRange;

    private Camera _camera;

    private readonly int _sizeProperty = Shader.PropertyToID("_Size");
    private readonly int _subSizeProperty = Shader.PropertyToID("_SubSize");

    private void Start()
    {
        _camera = EngineManager.Instance.EditorCamera;
        _subSize = _size / _gridBase;
    }

    private void CalculateRange()
    {
        _sizeRange = new Vector2(
            _maxSize * (_size / _gridBase) / 2f,
            _maxSize * _size / 2f
        );
    }

    private void Update()
    {
        if (!GridEnabled)
        {
            return;
        }

        HandleGridSize();
    }

    private void HandleGridSize()
    {
        float size = _camera.orthographicSize;
        if (size < _sizeRange.x)
        {
            _size = _subSize;
            _subSize = _size / _gridBase;
            UpdateGridSize();
            CalculateRange();
        }
        else if (size > _sizeRange.y)
        {
            _subSize = _size;
            _size = _subSize * _gridBase;
            UpdateGridSize();
            CalculateRange();
        }
    }

    private void UpdateGridSize()
    {
        _gridMaterial.SetFloat(_sizeProperty, _size);
        _gridMaterial.SetFloat(_subSizeProperty, _subSize);
    }

    public void ToggleGrid()
    {
        GridEnabled = !GridEnabled;
        _gridOverlay.Toggle(GridEnabled);

        if (GridEnabled)
        {
            CalculateRange();
            UpdateGridSize();
        }
    }

    public void ToogleSnap()
    {
        SnapEnabled = !SnapEnabled;
    }

    public Vector3 GetPosition(Vector3 position)
    {
        if (GridEnabled && SnapEnabled)
        {
            float x = Mathf.Round(position.x / _subSize) * _subSize;
            float y = Mathf.Round(position.y / _subSize) * _subSize;
            float z = Mathf.Round(position.z / _subSize) * _subSize;
            return new Vector3(x, y, z);
        }

        return position;
    }
}
