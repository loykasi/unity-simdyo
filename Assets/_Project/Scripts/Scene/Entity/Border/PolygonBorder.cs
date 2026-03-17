using System;
using System.Collections.Generic;
using UnityEngine;

public class PolygonBorder : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _renderer;

    private readonly int _colorProperty = Shader.PropertyToID("_Color");

    private void Awake()
    {
        Disable();
    }

    public void Enable(ColorHSV color)
    {
        float x = Mathf.Max(color.V - 0.75f, 0) / 0.25f;
        float threshold = Mathf.Lerp(0f, 0.35f, x);

        if (color.S < threshold)
        {
            _renderer.material.SetColor(_colorProperty, Color.black);
        }
        else
        {
            _renderer.material.SetColor(_colorProperty, Color.white);
        }

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
}