using UnityEngine;

public class CircleHighlight : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _renderer;
    private MeshWrapper _mesh;
    private readonly int _sizeProperty = Shader.PropertyToID("_Size");

    private void Awake()
    {
        Disable();
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Create(float radius)
    {
        SetMesh(CircleMeshGenerator.Generate(radius));
        _renderer.material.SetFloat(_sizeProperty, radius);
    }

    public void SetMesh(MeshWrapper meshWrapper)
    {
        _mesh = meshWrapper;
        meshWrapper.AssignTo(_meshFilter);
    }

    public void SetRadius(float radius)
    {
        _renderer.material.SetFloat(_sizeProperty, radius);

        int length = ShapeGenerator.TotalVert;
        float angleStep = 2 * Mathf.PI / length;
        float vertRadius = radius / Mathf.Cos(Mathf.PI / length);
        for (int i = 0; i < length; i++)
        {
            float angle = i * angleStep;
            _mesh.Vertices[i] = new Vector3
            (
                vertRadius * Mathf.Sin(angle),
                vertRadius * Mathf.Cos(angle),
                0f
            );
        }
        _mesh.Update();
    }
}