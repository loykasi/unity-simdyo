using UnityEngine;
using UnityEngine.InputSystem;

public class CircleTool : ITool
{
    private bool _onMouseMove;
    private Vector3 _startPosition;


    public void OnUpdate(Vector3 mousePosition)
    {
        Create(mousePosition);
    }

    private void Create(Vector3 mousePosition)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !ScreenInteractionUtils.IsOverUI())
        {
            _startPosition = mousePosition;

            _onMouseMove = true;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && _onMouseMove)
        {
            _onMouseMove = false;

            ShapeGenerator.Instance.AddCircle(_startPosition, mousePosition);
        }

        if (_onMouseMove)
        {
            Debug.DrawRay(_startPosition, Vector3.up, Color.red);
            Debug.DrawRay(mousePosition, Vector3.up, Color.red);
        }
    }
}