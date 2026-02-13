using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EntityContextMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private RectTransform _canvas;
    private bool _isHover = false;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !_isHover)
        {
            gameObject.SetActive(false);
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

    public void Delete()
    {
        ObjectManager.Instance.DeleteCurrent();
        gameObject.SetActive(false);
    }

    public void Intersect()
    {
        ObjectManager.Instance.DoIntersection();
        gameObject.SetActive(false);
    }

    public void Subtract()
    {
        ObjectManager.Instance.DoSubtract();
        gameObject.SetActive(false);
    }
}