using UnityEngine;

public class UIResizeBounds : MonoBehaviour
{
    public void BeginDrag(BoundsHandleDirection direction)
    {
        ResizeController.Instance.BeginResize(direction);
    }
    
    public void Drag(BoundsHandleDirection direction, Vector3 position)
    {
        ResizeController.Instance.Resize(direction, position);
    }
}