using Loykas.Scripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UISceneGame : MonoBehaviour
{
    [SerializeField] private GameObject[] _uiElements;

    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleHideUI();
        }
    }

    public void OnActionStopClicked()
    {
        EngineManager.Instance.RequestStop();
    }

    public void OnActionConsoleClicked()
    {
        LogConsole.Instance.Toggle();
    }
    
    public void OnActionHideUIClicked()
    {
        ToggleHideUI();
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