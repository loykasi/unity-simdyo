using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ColorPickerWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private ColorProperty[] _colorProperties;
    private bool _isHover = false;
    private Vector3 _offsetFromMouse;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !_isHover)
        {
            Close();
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
        
        Vector3 worldPosition = Mouse.current.position.ReadValue();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, worldPosition, null, out Vector2 point);
        Vector2 position = point + new Vector2(_canvas.sizeDelta.x * 0.5f, _canvas.sizeDelta.y * 0.5f);

        Debug.Log($"world: {worldPosition}");
        Debug.Log($"screen: {position}");

        float xDiff = position.x + _rect.sizeDelta.x - _canvas.sizeDelta.x;
        float yDiff = _canvas.sizeDelta.y - position.y - _rect.sizeDelta.y;

        if (xDiff > 0)
        {
            position.x -= xDiff;
        }

        if (yDiff > 0)
        {
            position.y += yDiff;
        }

        _rect.anchoredPosition = position;

        Init();
    }

    public void Init()
    {
        foreach (var property in _colorProperties)
        {
            property.Init();
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        ColorPickerController.Instance.Close();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHover = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _offsetFromMouse = transform.position - (Vector3)eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = (Vector3)eventData.position + _offsetFromMouse;
    }
}