using System;
using Loykas.Scripting;
using UnityEngine;
using UnityEngine.UI;

public class FunctionMenuInput : MonoBehaviour
{
    public string Name => (string)_nameInput.GetValue();
    public ScriptDataType Type => (ScriptDataType)_typeInput.GetValue();

    [SerializeField] private StringInput _nameInput;
    [SerializeField] private TypeInput2 _typeInput;
    [SerializeField] private Button _deleteButton;

    private FunctionMenu _menu;

    private void Awake()
    {
        _nameInput.OnSubmit += OnNameUpdated;
        _typeInput.OnSubmit += OnTypeUpdated;
        _deleteButton.onClick.AddListener(Delete);
    }

    public void Init(FunctionMenu menu, string name, ScriptDataType type)
    {
        _menu = menu;
        _nameInput.SetValue(name);
        _typeInput.SetValue(type);
    }

    private void OnNameUpdated(object value)
    {
        _menu.UpdateInput(this);
    }

    private void OnTypeUpdated(object value)
    {
        _menu.UpdateInput(this);
    }

    private void Delete()
    {
        _menu.DeleteInput(this);
    }
}