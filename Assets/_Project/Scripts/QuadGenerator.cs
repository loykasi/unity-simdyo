using UnityEngine;

public class QuadGenerator : MonoBehaviour
{
    [SerializeField] private ShapeGenerator _shapeGenerator;

    private void Awake()
    {
        _shapeGenerator.AddBox();
        _shapeGenerator.AddCircle();
    }
}
