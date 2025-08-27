using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TextureMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _isMouseOver = false;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !_isMouseOver)
        {
            AssetController.Instance.CloseTextureMenu();
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMouseOver = false;
    }
}