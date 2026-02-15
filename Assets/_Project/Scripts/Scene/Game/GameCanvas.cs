using UnityEngine;
using UnityEngine.InputSystem;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private GameObject[] _uiElements;

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleHideUI();
        }
    }

    public void ToggleHideUI()
    {
        for (int i = 0; i < _uiElements.Length; i++)
        {
            GameObject element = _uiElements[i];
            element.SetActive(!element.activeSelf);
        }
    }
}