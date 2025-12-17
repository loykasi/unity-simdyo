using UnityEngine;
using UnityEngine.EventSystems;

public class CursorHover : MonoBehaviour, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private CursorType _cursorType;
    [SerializeField] private int _priority;

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CursorSystem.Instance != null)
        {
            CursorSystem.Instance.ToDefault(_priority);    
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (CursorSystem.Instance != null)
        {
            CursorSystem.Instance.SetCursor(_cursorType, _priority);
        }
    }
}