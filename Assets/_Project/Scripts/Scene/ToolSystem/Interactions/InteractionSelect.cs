using System;
using UnityEngine;

public class InteractionSelect
{
    private Vector2 _startMousePosition;
    
    private Action _clickEvent;
    private Action _clickReleasedEvent;


    public InteractionSelect()
    {
        _clickEvent = OnClick;
        _clickReleasedEvent = OnClickReleased;
    }

    public void Enable()
    {
        InputManager.Instance.ClickEvent += _clickEvent;
        InputManager.Instance.ClickReleasedEvent += _clickReleasedEvent;
    }

    public void Disable()
    {
        InputManager.Instance.ClickEvent -= _clickEvent;
        InputManager.Instance.ClickReleasedEvent -= _clickReleasedEvent;
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
        }
    }
}