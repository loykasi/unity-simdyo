using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuToggle : MonoBehaviour
{
    public UnityEvent OnOpened;
    public UnityEvent OnClosed;

    [SerializeField] private GameObject _menu;
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(ToggleMenu);
    }

    public void ToggleMenu()
    {
        _menu.SetActive(!_menu.activeSelf);

        if (_menu.activeSelf)
        {
            OnOpened?.Invoke();
        }
        else
        {
            OnClosed?.Invoke();
        }
    }
}