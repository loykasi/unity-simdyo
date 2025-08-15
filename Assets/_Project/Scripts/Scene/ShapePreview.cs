using UnityEngine;

public class ShapePreview : Singleton<ShapePreview>
{
    [SerializeField] private MeshFilter _boxPreview;
    private Mesh _boxMesh;

    [SerializeField] private MeshFilter _circlePreview;
    [SerializeField] private MeshRenderer _circleRenderer;
    private Mesh _circleMesh;

    [SerializeField] private int _totalVert;

    private readonly int _radiusProperty = Shader.PropertyToID("_Radius");

    public void StartBoxPreview()
    {
        _boxPreview.gameObject.SetActive(true);
        _boxMesh = _boxPreview.mesh;
    }

    public void PreviewBox(Vector3 from, Vector3 to)
    {
        Vector3 center = (from + to) / 2f;
        float halfWidth = Mathf.Abs(from.x - to.x) / 2f;
        float halfHeight = Mathf.Abs(from.y - to.y) / 2f;

        _boxPreview.transform.position = center;
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(- halfWidth, halfHeight),
            new Vector3(halfWidth, halfHeight),
            new Vector3(- halfWidth, - halfHeight),
            new Vector3(halfWidth, - halfHeight),
        };
        _boxMesh.vertices = vertices;
    }

    public void StopBoxPreview()
    {
        _boxPreview.gameObject.SetActive(false);
    }

    public void StartCirclePreview()
    {
        _circlePreview.gameObject.SetActive(true);
        _circleMesh = _circlePreview.mesh;
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
    }

    public void StopCirclePreview()
    {
        _circlePreview.gameObject.SetActive(false);
    }
}