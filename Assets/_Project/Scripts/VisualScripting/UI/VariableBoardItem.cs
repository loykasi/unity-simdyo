using System;
using System.Collections.Generic;
using Loykas.Scripting;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VariableBoardItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private TMP_Dropdown _typeDropdown;
    [SerializeField] private Button _removeButton;

    [SerializeField] private VariableInput[] _variableInputs;
    private VariableInput _currentInput;

    private VariableBoard _variableBoard;
    private VisualScripting _vs;

    private readonly float _width = 300f;
    private readonly float _verticalPadding = 10f;
    private readonly float _titleAndTypeHeight = 60f;

    private void Start()
    {
        InitDropDown();

        _typeDropdown.onValueChanged.AddListener(OnTypeChanged);
        _removeButton.onClick.AddListener(OnRemove);

        foreach (var item in _variableInputs)
        {
            item.VariableItem = this;
        }

        for (int i = 0; i < _variableInputs.Length; i++)
        {
            _variableInputs[i].OnValueUpdated += UpdateVariable;
        }
    }

    private void InitDropDown()
    {
        if (_typeDropdown.options.Count != 0)
        {
            return;
        }

        _typeDropdown.AddOptions(DataTypeController.Instance.DataTypesDropdownValues);
    }

    public void Init(string name, VariableBoard variableBoard)
    {
        _nameInputField.text = name;
        _typeDropdown.value = 0;

        _variableBoard = variableBoard;
        OnTypeChanged(0);

        _vs = NodeBoard.Instance.TargetVisualScripting;

        for (int i = 0; i < _variableInputs.Length; i++)
        {
            _variableInputs[i].Disable();
        }
        _currentInput = _variableInputs[0];
        _currentInput.Enable();
    }

    public void Init(string name, DataType type, ListType? subType, object value, VariableBoard variableBoard)
    {
        InitDropDown();

        int typeIndex = GetDataTypeIndex(type, subType);

        _nameInputField.text = name;
        _typeDropdown.SetValueWithoutNotify(typeIndex);

        _variableBoard = variableBoard;

        for (int i = 0; i < _variableInputs.Length; i++)
        {
            _variableInputs[i].Disable();
        }
        _currentInput = _variableInputs[typeIndex];
        _currentInput.Enable();

        _currentInput.SetValue(value);

        _vs = NodeBoard.Instance.TargetVisualScripting;
    }

    private void OnTypeChanged(int index)
    {
        for (int i = 0; i < _variableInputs.Length; i++)
        {
            _variableInputs[i].Disable();
        }
        _currentInput = _variableInputs[index];
        _currentInput.Enable();

        UpdateVariable();
    }

    private void UpdateVariable()
    {
        DataType type = _currentInput.Type;
        if (type != DataType.List)
        {
            _variableBoard.UpdateVariable(_nameInputField.text, type, _currentInput.GetValue());
        }
        else
        {
            UpdateListVariable();
        }

        UpdateSize();
    }

    // private void UpdateListVariable()
    // {
    //     VariableInput input = _variableInputs[_currentType];
    //     _variableBoard.UpdateVariable(_nameInputField.text, GetDataType(), input.GetValue());
    // }

    public void UpdateListVariable()
    {
        var input = (ListInputField)_currentInput;
        _vs.UpdateListVariable(_nameInputField.text, input.SubType);
    }

    public void InsertListItem(object value)
    {
        _vs.InsertListItem(_nameInputField.text, value);
        UpdateSize();
    }

    public void UpdateListItem(int index, object value)
    {
        _vs.UpdateListItem(_nameInputField.text, index, value);
    }

    public void RemoveListItem(int index)
    {
        _vs.RemoveListItem(_nameInputField.text, index);
        UpdateSize();
    }

    private int GetDataTypeIndex(DataType type, ListType? subtype)
    {
        return type switch
        {
            DataType.String => 0,
            DataType.Number => 1,
            DataType.Boolean => 2,
            DataType.Vector => 3,
            DataType.Color => 4,
            DataType.List => subtype switch
            {
                ListType.String => 5,
                ListType.Number => 6,
                ListType.Boolean => 7,
                _ => throw new NotImplementedException(),
            },
            _ => throw new NotImplementedException(),
        };
    }

    private void OnRemove()
    {
        _variableBoard.RemoveVariable(_nameInputField.text, this);
    }

    private void UpdateSize()
    {
        _rect.sizeDelta = new Vector2
        (
            _width,
            _verticalPadding + _titleAndTypeHeight + _currentInput.Height
        );
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Debug.Log("drag variable");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}