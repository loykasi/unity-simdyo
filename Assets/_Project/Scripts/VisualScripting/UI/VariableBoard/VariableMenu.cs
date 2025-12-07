using System;
using Loykas.Scripting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VariableMenu : MonoBehaviour
{
    [Header("Modal")]
    [SerializeField] private GameObject _window;
    [SerializeField] private RectTransform _windowRect;
    [SerializeField] private Button _closeButton;

    [Header("Input")]    
    [SerializeField] private StringInput _nameInput;
    [SerializeField] private TypeInput _typeInput;

    [SerializeField] private RectTransform _deleteGroupRect;
    [SerializeField] private Button _deleteButton;

    [SerializeField] private RectTransform _valueRect;
    [SerializeField] private RectTransform _inputHolder;
    [SerializeField] private UIInputData _inputDataReference;
    private BaseInput _input;

    private ScriptFlow _flow;
    private Variable _variable;

    private readonly float _width = 400f;
    private readonly float _verticalPadding = 10f;
    private readonly float _titleAndTypeHeight = 60f;

    private void Awake()
    {
        _closeButton.onClick.AddListener(Close);
        _deleteButton.onClick.AddListener(DeleteVariable);
    }

    private void OnEnable()
    {
        VariableBoard.OnSelectItem += OnSelectItem;
        _nameInput.OnSubmit += OnNameChanged;
        _typeInput.OnSubmit += OnTypeChanged;
    }

    private void OnDisable()
    {
        VariableBoard.OnSelectItem -= OnSelectItem;
        _nameInput.OnSubmit -= OnNameChanged;
        _typeInput.OnSubmit -= OnTypeChanged;
    }

    private void OnNameChanged(object value)
    {
        string name = (string)value;
        _flow.ChangeVariableName(_variable.Name, name);
    }

    private void OnSelectItem(ScriptFlow flow, Variable variable)
    {
        _window.SetActive(true);

        _flow = flow;
        _variable = variable;

        OnVariableUpdated();
        UpdateSize();
    }

    private void Close()
    {
        _window.SetActive(false);
    }

    private void OnVariableUpdated()
    {
        _nameInput.SetValue(_variable.Name);
        _typeInput.SetValue(_variable.Type);
        ChangeInput(_variable.Type);
    }
    
    private void OnTypeChanged(object value)
    {
        ScriptDataType type = (ScriptDataType)value;

        ValueHandler.SetDefaultValue(_variable, type);

        ChangeInput(type);
    }

    private void ChangeInput(ScriptDataType type)
    {
        if (_input != null)
        {
            Destroy(_input.gameObject);
        }

        _input = _inputDataReference.Get(type);
        _input.Rect.SetParent(_inputHolder, false);
        _input.Enable();

        _input.OnValueUpdated += OnValueUpdated;
        _input.SetValueInstance(_variable);
    }

    private void OnValueUpdated()
    {
        UpdateSize();
    }

    private void UpdateSize()
    {
        float diff = _input.Size.y - _valueRect.sizeDelta.y;
        diff += _verticalPadding;
        
        _valueRect.sizeDelta += new Vector2(0f, diff);
        _deleteGroupRect.anchoredPosition += new Vector2(0f, -diff);
        _windowRect.sizeDelta += new Vector2(0f, diff);
    }
    
    private void DeleteVariable()
    {
        _flow.RemoveVariable(_variable);
        _variable = null;
        Close();
    }
}