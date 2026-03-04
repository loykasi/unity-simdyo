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
    [SerializeField] private bool _shouldMatchX;

    private void Awake()
    {
        _button.onClick.AddListener(ToggleMenu);
    }

    public void ToggleMenu()
    {
        if (_shouldMatchX)
        {
            _menu.transform.position = new Vector2(transform.position.x, _menu.transform.position.y);
        }
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