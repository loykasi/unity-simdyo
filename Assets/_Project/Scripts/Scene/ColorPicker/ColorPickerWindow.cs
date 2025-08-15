using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ColorPickerWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _isHover = false;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !_isHover)
        {
            Close();
        }
    }

    public void Close()
    {
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
}