using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragPreviewSystem : Singleton<DragPreviewSystem>
{
    [SerializeField] private RectTransform _previewObject;
    [SerializeField] private TMP_Text _previewText;
    
    private Vector2 _offset;

    public void Update()
    {
        if (_previewObject.gameObject.activeInHierarchy)
        {
            _previewObject.position = Mouse.current.position.ReadValue() + _offset;
            CursorSystem.Instance.SetCursor(CursorType.Grabbing, 2);
        }
    }

    public void BeginDrag(Vector2 position, string name = "")
    {
        _offset = position - Mouse.current.position.ReadValue();
        _previewObject.gameObject.SetActive(true);
        _previewText.text = name;
    }

    public void EndDrag()
    {
        _previewObject.gameObject.SetActive(false);

        CursorSystem.Instance.ToDefault(2);
    }
}