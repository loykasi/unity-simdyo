using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoxTool : ITool
{
    private bool _onMouseMove;
    private Vector3 _startPosition;


    public void OnUpdate(Vector3 mousePosition)
    {
        Create(mousePosition);
    }

    private void Create(Vector3 mousePosition)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _startPosition = mousePosition;

            _onMouseMove = true;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && _onMouseMove)
        {
            _onMouseMove = false;

            ShapeGenerator.Instance.AddBox(_startPosition, mousePosition);
        }

        if (_onMouseMove)
        {
            Debug.DrawRay(_startPosition, Vector3.up, Color.red);
            Debug.DrawRay(mousePosition, Vector3.up, Color.red);
        }
    }
}