using UnityEngine;
using UnityEngine.InputSystem;

public class CircleTool : PanTool
{
    public override ToolType Type => ToolType.Circle;

    private bool _onMouseMove;
    private Vector3 _startPosition;

    public override void OnUpdate()
    {
        Zoom();
        HandlePanRightMouse();
        Create();
    }

    private void Create()
    {
        Vector3 mousePosition = GetMouseWorldPositon();
        if (Mouse.current.leftButton.wasPressedThisFrame && !ScreenInteractionUtils.IsOverUI())
        {
            _startPosition = Vector3Utils.GetGridPosition(mousePosition);

            _onMouseMove = true;
            ShapePreview.Instance.StartCirclePreview();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && _onMouseMove)
        {
            _onMouseMove = false;

            ShapeGenerator.Instance.AddCircle(_startPosition, Vector3Utils.GetGridPosition(mousePosition));
            ShapePreview.Instance.StopCirclePreview();
        }

        if (_onMouseMove)
        {
            Debug.DrawRay(_startPosition, Vector3.up, Color.red);
            Debug.DrawRay(Vector3Utils.GetGridPosition(mousePosition), Vector3.up, Color.red);
            ShapePreview.Instance.PreviewCircle(_startPosition, Vector3Utils.GetGridPosition(mousePosition));
        }
    }
}