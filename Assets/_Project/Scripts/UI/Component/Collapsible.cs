using UnityEngine;

public class Collapsible : MonoBehaviour
{
    public bool IsOpened;
    [SerializeField] private RectTransform _content;

    public void Toggle()
    {
        SetOpen(!IsOpened);
    }

    public void SetOpen(bool open)
    {
        if (_content == null) return;
        
        IsOpened = open;
        _content.gameObject.SetActive(IsOpened);
    }
}