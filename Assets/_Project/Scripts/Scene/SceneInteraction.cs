using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SceneInteraction : MonoBehaviour
{
    public List<RaycastResult> raycastResults = new();

    private void Update()
    {
        HandleSelection();
    }

    private void HandleSelection()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (ScreenInteractionUtils.IsOverUI())
            {
                return;
            }
            
            Vector3 mousePosition = Mouse.current.position.ReadValue();

            ObjectManager.Instance.Select(mousePosition);
        }
    }
}