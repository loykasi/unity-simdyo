using UnityEngine;

public class BoxTool : BaseTool
{
    public override ToolType Type => ToolType.Box;

    private bool _onMouseMove;
    private Vector3 _startPosition;

    public override void Disable()
    {
        if (_onMouseMove)
        {
            _onMouseMove = false;
            ShapePreview.Instance.StopBoxPreview();
        }
    }

    public override bool OnPointerLeftDown(Vector2 position)
    {
        Vector2 mousePosition = Utils.ToWorldPositon(InputManager.Instance.MousePosition);
        _startPosition = Vector3Utils.GetGridPosition(mousePosition);

        _onMouseMove = true;
        ShapePreview.Instance.StartBoxPreview();
        ShapePreview.Instance.PreviewBox(_startPosition, _startPosition);

        return true;
    }

    public override void OnPointerLeftUp(Vector2 position)
    {
        if (_onMouseMove)
        {
            _onMouseMove = false;

            Vector2 mousePosition = Utils.ToWorldPositon(InputManager.Instance.MousePosition);;
            ObjectManager.Instance.AddBox(_startPosition, Vector3Utils.GetGridPosition(mousePosition));
            ShapePreview.Instance.StopBoxPreview();
        }
    }

    public override void OnPointerMove(Vector2 value)
    {
        if (_onMouseMove)
        {
            Vector2 mouseWorldPostiion = Utils.ToWorldPositon(value);
            ShapePreview.Instance.PreviewBox(_startPosition, Vector3Utils.GetGridPosition(mouseWorldPostiion));
            
            Debug.DrawRay(_startPosition, Vector3.up, Color.red);
            Debug.DrawRay(Vector3Utils.GetGridPosition(mouseWorldPostiion), Vector3.up, Color.red);
        }
    }
}