using UnityEngine;
using UnityEngine.EventSystems;

public class UIResizeHandle : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public BoundsHandleDirection Direction;

    [SerializeField] private UIResizeBounds _resizeBounds;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _resizeBounds.BeginDrag(Direction);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _resizeBounds.Drag(Direction, eventData.position);
    }
}