using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIContextMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private UIContextMenuEntry[] _entries;
    private bool _isHover = false;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !_isHover)
        {
            Close();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHover = false;
    }

    public void Open()
    {
        gameObject.SetActive(true);
        Init(ObjectManager.Instance.SelectionGroup);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);
        PositionAsPointer();
    }

    private void Init(EntityGroup group)
    {
        foreach (UIContextMenuEntry entry in _entries)
        {
            ContextMenuAction action = ContextMenuManager.Instance.GetAction(entry.Type);
            bool isActive = action != null && action.IsAvailable(group);
            entry.gameObject.SetActive(isActive);
        }
    }

    private void PositionAsPointer()
    {
        Vector3 worldPosition = Mouse.current.position.ReadValue();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, worldPosition, null, out Vector2 point);
        Vector2 position = point + new Vector2(_canvas.sizeDelta.x * 0.5f, _canvas.sizeDelta.y * 0.5f);
        
        float xDiff = position.x + _rect.sizeDelta.x - _canvas.sizeDelta.x;
        float yDiff = _rect.sizeDelta.y - position.y;

        if (xDiff > 0)
        {
            position.x -= xDiff;
        }

        if (yDiff > 0)
        {
            position.y += yDiff;
        }

        _rect.anchoredPosition = position;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
