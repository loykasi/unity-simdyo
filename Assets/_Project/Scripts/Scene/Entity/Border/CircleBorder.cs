using UnityEngine;

public class CircleBorder : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _renderer;

    private readonly int _radiusProperty = Shader.PropertyToID("_Radius");

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

    public void SetMesh(MeshWrapper meshWrapper)
    {
        meshWrapper.AssignTo(_meshFilter);
    }

    public void SetRadius(float radius)
    {
        _renderer.material.SetFloat(_radiusProperty, radius);
    }
}