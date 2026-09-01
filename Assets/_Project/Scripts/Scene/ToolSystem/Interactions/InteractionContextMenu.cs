using System;
using UnityEngine;

public class InteractionContextMenu
{
    private Vector2 _startMousePosition;
    
    private Action _clickEvent;
    private Action _clickReleasedEvent;


    public InteractionContextMenu()
    {
        _clickEvent = OnClick;
        _clickReleasedEvent = OnClickReleased;
    }

    public void Enable()
    {
        InputManager.Instance.RightClickEvent += _clickEvent;
        InputManager.Instance.RightClickReleasedEvent += _clickReleasedEvent;
    }

    public void Disable()
    {
        InputManager.Instance.RightClickEvent -= _clickEvent;
        InputManager.Instance.RightClickReleasedEvent -= _clickReleasedEvent;
    }

    private void OnClick()
    {
        _startMousePosition = InputManager.Instance.MousePosition;

        if (ScreenInteractionUtils.IsOverUI())
        {
            return;
        }
    }

    private void OnClickReleased()
    {
        if (ScreenInteractionUtils.IsOverUI())
        {
            return;
        }
        
        if (_startMousePosition == InputManager.Instance.MousePosition)
        {
            ObjectManager.Instance.Select(_startMousePosition);
            EntityContextMenuController.Instance.Open();
        }
    }
}