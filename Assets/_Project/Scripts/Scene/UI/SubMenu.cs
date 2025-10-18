using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SubMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _isHover;

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
}