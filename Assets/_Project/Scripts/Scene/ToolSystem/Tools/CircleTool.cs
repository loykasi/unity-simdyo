using UnityEngine;

public class CircleTool : BaseTool
{
    public override ToolType Type => ToolType.Circle;

    private bool _onMouseMove;
    private Vector3 _startPosition;

    protected override void OnClick()
    {
        if (ScreenInteractionUtils.IsOverUI())
        {
            return;
        }

        Vector2 mousePosition = Utils.ToWorldPositon(InputManager.Instance.MousePosition);
        _startPosition = Vector3Utils.GetGridPosition(mousePosition);

        _onMouseMove = true;
        ShapePreview.Instance.StartCirclePreview();
        ShapePreview.Instance.PreviewCircle(_startPosition, Vector3Utils.GetGridPosition(mousePosition));
    }

    protected override void OnClickReleased()
    {
        if (_onMouseMove)
        {
            _onMouseMove = false;

            Vector2 mousePosition = Utils.ToWorldPositon(InputManager.Instance.MousePosition);;
            ObjectManager.Instance.AddCircle(_startPosition, Vector3Utils.GetGridPosition(mousePosition));
            ShapePreview.Instance.StopCirclePreview();
        }
    }

    protected override void OnPointMove(Vector2 value)
    {
        base.OnPointMove(value);

        if (_onMouseMove)
        {
            Vector2 mouseWorldPostiion = Utils.ToWorldPositon(value);
            ShapePreview.Instance.PreviewCircle(_startPosition, Vector3Utils.GetGridPosition(mouseWorldPostiion));

            // Debug.DrawRay(_startPosition, Vector3.up, Color.red);
            // Debug.DrawRay(Vector3Utils.GetGridPosition(mouseWorldPostiion), Vector3.up, Color.red);
        }
    }
}