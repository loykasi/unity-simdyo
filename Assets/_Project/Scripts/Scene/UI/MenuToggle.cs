using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuToggle : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.onClick.AddListener(ToggleMenu);
    }

    private void ToggleMenu()
    {
        _menu.SetActive(!_menu.activeSelf);
    }
}