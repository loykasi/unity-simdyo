using UnityEngine;
using UnityEngine.InputSystem;

public class BoxTool : ITool
{
    private bool _onMouseMove;
    private Vector3 _startPosition;

    public void Disable()
    {
        
    }

    public void Enable()
    {
        
    }

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
            ShapePreview.Instance.StartBoxPreview();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && _onMouseMove)
        {
            _onMouseMove = false;

            ShapePreview.Instance.StopBoxPreview();
            ShapeGenerator.Instance.AddBox(_startPosition, mousePosition);
        }

        if (_onMouseMove)
        {
            Debug.DrawRay(_startPosition, Vector3.up, Color.red);
            Debug.DrawRay(mousePosition, Vector3.up, Color.red);
            ShapePreview.Instance.PreviewBox(_startPosition, mousePosition);
        }
    }
}