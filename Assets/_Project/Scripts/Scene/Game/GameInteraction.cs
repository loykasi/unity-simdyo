using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInteraction : MonoBehaviour
{
    private void Update()
    {
        HandleSelection();
    }

    private void HandleSelection()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {            
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            ObjectManager.Instance.Click(mousePosition);
        }
    }
}