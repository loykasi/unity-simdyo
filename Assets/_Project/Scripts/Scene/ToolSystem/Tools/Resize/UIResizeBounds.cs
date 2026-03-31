using UnityEngine;

public class UIResizeBounds : MonoBehaviour
{
    [SerializeField] private ResizeTool _resizeTool;

    public void BeginDrag(BoundsHandleDirection direction)
    {
        _resizeTool.BeginResize(direction);
    }
    
    public void Drag(BoundsHandleDirection direction, Vector3 position)
    {
        _resizeTool.Resize(direction, position);
    }
}