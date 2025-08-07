using UnityEngine;
using UnityEngine.InputSystem;

public class MoveTool : ITool
{
    private bool _onMovingObject = false;
    private Vector3 _offsetFromMouse;

    public void OnUpdate(Vector3 mousePosition)
    {
        HandleMoving(mousePosition);
    }
    
    private void HandleMoving(Vector3 mousePosition)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !ScreenInteractionUtils.IsOverUI())
        {
            var selected = ObjectManager.Instance.SelectedObject;
            if (selected == null)
            {
                return;
            }
            _offsetFromMouse = selected.transform.position - mousePosition;
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
            selected.transform.position = mousePosition + _offsetFromMouse;
        }
    }
}