using UnityEngine;

public class BoxTool : BaseTool
{
    public override ToolType Type => ToolType.Box;

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
        ShapePreview.Instance.StartBoxPreview();
        ShapePreview.Instance.PreviewBox(_startPosition, _startPosition);
    }

    protected override void OnClickReleased()
    {
        if (_onMouseMove)
        {
            _onMouseMove = false;

            Vector2 mousePosition = Utils.ToWorldPositon(InputManager.Instance.MousePosition);;
            ObjectManager.Instance.AddBox(_startPosition, Vector3Utils.GetGridPosition(mousePosition));
            ShapePreview.Instance.StopBoxPreview();   
        }
    }

    protected override void OnPointMove(Vector2 value)
    {
        base.OnPointMove(value);

        if (_onMouseMove)
        {
            Vector2 mouseWorldPostiion = Utils.ToWorldPositon(value);
            ShapePreview.Instance.PreviewBox(_startPosition, Vector3Utils.GetGridPosition(mouseWorldPostiion));
            
            Debug.DrawRay(_startPosition, Vector3.up, Color.red);
            Debug.DrawRay(Vector3Utils.GetGridPosition(mouseWorldPostiion), Vector3.up, Color.red);
        }
    }
}