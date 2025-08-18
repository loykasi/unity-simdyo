using UnityEngine;
using UnityEngine.InputSystem;

public class BoxTool : PanTool
{
    public override ToolType Type => ToolType.Box;

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
            ShapePreview.Instance.StartBoxPreview();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && _onMouseMove)
        {
            _onMouseMove = false;

            ShapePreview.Instance.StopBoxPreview();
            ShapeGenerator.Instance.AddBox(_startPosition, Vector3Utils.GetGridPosition(mousePosition));
        }

        if (_onMouseMove)
        {
            Debug.DrawRay(_startPosition, Vector3.up, Color.red);
            Debug.DrawRay(Vector3Utils.GetGridPosition(mousePosition), Vector3.up, Color.red);
            ShapePreview.Instance.PreviewBox(_startPosition, Vector3Utils.GetGridPosition(mousePosition));
        }
    }
}