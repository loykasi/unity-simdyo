using UnityEngine;
using UnityEngine.InputSystem;

public class MoveTool : PanTool
{
    public override ToolType Type => ToolType.Move;

    private bool _onMovingObject = false;
    private Vector3 _offsetFromMouse;

    public override void OnUpdate()
    {
        base.Zoom();
        base.HandlePanRightMouse();
        Move();
    }

    public void Move()
    {
        Vector3 mousePosition = GetMouseWorldPositon();

        if (Mouse.current.leftButton.wasPressedThisFrame && !ScreenInteractionUtils.IsOverUI())
        {
            var selected = ObjectManager.Instance.SelectedObject;
            if (selected == null)
            {
                return;
            }
            _offsetFromMouse = selected.transform.position - Vector3Utils.GetGridPosition(mousePosition);
            _onMovingObject = true;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _onMovingObject = false;
            Physics2D.SyncTransforms();
        }

        if (_onMovingObject)
        {
            var selected = ObjectManager.Instance.SelectedObject;
            selected.transform.position = Vector3Utils.GetGridPosition(mousePosition) + _offsetFromMouse;
            Debug.Log(Vector3Utils.GetGridPosition(mousePosition));
        }
    }
}