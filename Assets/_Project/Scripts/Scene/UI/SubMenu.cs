using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SubMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool _shouldCloseOnClickOutside;
    [SerializeField] private MenuToggle _menuToggle;
    private bool _isHover;

    private void Update()
    {
        if (_shouldCloseOnClickOutside && 
            Mouse.current.leftButton.wasPressedThisFrame &&
            !_isHover)
        {
            _menuToggle.ToggleMenu();
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