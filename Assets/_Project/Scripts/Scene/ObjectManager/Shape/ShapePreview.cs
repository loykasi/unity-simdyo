using UnityEngine;

public class ShapePreview : Singleton<ShapePreview>
{
    [SerializeField] private MeshFilter _boxPreview;
    private Vector3[] _boxVertices = new Vector3[4];
    private Mesh _boxMesh;

    [SerializeField] private MeshFilter _circlePreview;
    [SerializeField] private MeshRenderer _circleRenderer;
    private Mesh _circleMesh;

    [SerializeField] private int _totalVert;

    [Header("Polygon")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _baseWidth = 0.05f;
    private int _pointCount = 1;

    private readonly int _radiusProperty = Shader.PropertyToID("_Radius");

    private void Start()
    {
        _boxMesh = _boxPreview.mesh;
        _circleMesh = _circlePreview.mesh;
    }

#region Box

    public void StartBoxPreview()
    {
        _boxPreview.gameObject.SetActive(true);
    }

    public void PreviewBox(Vector3 from, Vector3 to)
    {
        Vector3 center = (from + to) / 2f;
        float halfWidth = Mathf.Abs(from.x - to.x) / 2f;
        float halfHeight = Mathf.Abs(from.y - to.y) / 2f;

        _boxPreview.transform.position = center;
        
        _boxVertices[0] = new Vector3(- halfWidth, halfHeight);
        _boxVertices[1] = new Vector3(halfWidth, halfHeight);
        _boxVertices[2] = new Vector3(- halfWidth, - halfHeight);
        _boxVertices[3] = new Vector3(halfWidth, - halfHeight);

        _boxMesh.SetVertices(_boxVertices);
        _boxMesh.RecalculateBounds();
    }

    public void StopBoxPreview()
    {
        _boxPreview.gameObject.SetActive(false);
    }

#endregion

#region Circle

    public void StartCirclePreview()
    {
        _circlePreview.gameObject.SetActive(true);
    }

    public void PreviewCircle(Vector3 from, Vector3 to)
    {
        Vector3 center = from;
        float radius = Vector3.Distance(from, to);

        _circlePreview.transform.position = center;

        float vertRadius = radius / Mathf.Cos(Mathf.PI / _totalVert);
        Vector3[] vertices = new Vector3[_totalVert];
        for (int i = 0; i < _totalVert; i++)
        {
            float x = vertRadius * Mathf.Sin(i * 2 * Mathf.PI / _totalVert);
            float y = vertRadius * Mathf.Cos(i * 2 * Mathf.PI / _totalVert);
            vertices[i] = new Vector3(x, y, 0f);
        }
        _circleMesh.vertices = vertices;
        _circleRenderer.material.SetFloat(_radiusProperty, radius);
        _circleMesh.RecalculateBounds();
    }

    public void StopCirclePreview()
    {
        _circlePreview.gameObject.SetActive(false);
    }

#endregion

#region Polygon

    public void StartPolygonPreview()
    {
        _pointCount = 1;
        _lineRenderer.positionCount = _pointCount;
        _lineRenderer.gameObject.SetActive(true);
    }

    public void AddPolygonPoint(Vector3 point)
    {
        _lineRenderer.SetPosition(_pointCount - 1, point);
        _pointCount += 1;
        _lineRenderer.positionCount = _pointCount;
    }

    public void SetLastPoint(Vector3 point)
    {
        _lineRenderer.SetPosition(_pointCount - 1, point);   
    }

    public void StopPolygonPreview()
    {
        _lineRenderer.gameObject.SetActive(false);
        _pointCount = 1;
    }

    private void Update()
    {
        if (_lineRenderer.gameObject.activeInHierarchy)
        {
            Camera camera = EngineManager.Instance.EditorCamera;
            float width = camera.orthographicSize / 5f * _baseWidth;
            _lineRenderer.widthMultiplier = width;
        }
    }

#endregion
}