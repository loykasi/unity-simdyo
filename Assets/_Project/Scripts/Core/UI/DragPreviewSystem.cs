using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DragPreviewSystem : Singleton<DragPreviewSystem>
{
    [SerializeField] private RectTransform _previewObject;
    [SerializeField] private TMP_Text _previewText;
    [SerializeField] private Image _image;
    [SerializeField] private Color _defaultColor;
    
    private Vector2 _offset;

    public void Update()
    {
        if (_previewObject.gameObject.activeInHierarchy)
        {
            _previewObject.position = Mouse.current.position.ReadValue() + _offset;
            CursorSystem.Instance.SetCursor(CursorType.Grabbing, 2);
        }
    }

    public void BeginDrag(Vector2 position, string name = "", Color? color = null)
    {
        _offset = position - Mouse.current.position.ReadValue();
        _previewObject.gameObject.SetActive(true);
        _previewText.text = name;

        _image.color = color ?? _defaultColor;
    }

    public void EndDrag()
    {
        _previewObject.gameObject.SetActive(false);

        CursorSystem.Instance.ToDefault(2);
    }
}