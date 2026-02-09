using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private TMP_Text _content;
    [SerializeField] private Vector2 _offsetFromMouse;

    private void Update()
    {
        UpdatePosition();
    }

    public void Toggle(bool value)
    {
        if (value)
        {
            UpdatePosition();
        }
        gameObject.SetActive(value);
    }

    public void SetContent(string value)
    {
        _content.text = value;
    }

    private void UpdatePosition()
    {
        Vector3 worldPosition = Mouse.current.position.ReadValue();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, worldPosition, null, out Vector2 point);
        Vector2 position = point + _offsetFromMouse + new Vector2(_canvas.sizeDelta.x * 0.5f, _canvas.sizeDelta.y * 0.5f);
        
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
}