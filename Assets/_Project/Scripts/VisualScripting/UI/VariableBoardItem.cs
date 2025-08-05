using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VariableBoardItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private const int _stringTypeIndex = 0;
    private const int _numberTypeIndex = 1;
    private const int _booleanTypeIndex = 2;
    private const int _vectorTypeIndex = 3;
    private const int _listTypeIndex = 4;

    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private TMP_Dropdown _typeDropdown;
    [SerializeField] private Button _removeButton;

    [SerializeField] private VariableInput[] _variableInputs;

    private int _currentType;
    private VariableBoard _variableBoard;
    private VisualScripting _vs;

    private void Awake()
    {
        _typeDropdown.onValueChanged.AddListener(OnTypeChanged);
        _removeButton.onClick.AddListener(OnRemove);

        foreach (var item in _variableInputs)
        {
            item.VariableItem = this;   
        }

        for (int i = 0; i < _vectorTypeIndex; i++)
        {
            _variableInputs[i].OnValueUpdated += UpdateVariable;
        }
    }

    public void Init(string name, VariableBoard variableBoard)
    {
        _nameInputField.text = name;
        _typeDropdown.value = _stringTypeIndex;

        _variableBoard = variableBoard;
        OnTypeChanged(_stringTypeIndex);

        _vs = NodeBoard.Instance.TargetVisualScripting;

        for (int i = 0; i < _variableInputs.Length; i++)
        {
            _variableInputs[i].Disable();
        }
        _variableInputs[_currentType].Enable();
    }

    public void Init(string name, DataType type, object value, VariableBoard variableBoard)
    {
        _currentType = GetDataTypeIndex(type);

        _nameInputField.text = name;
        _typeDropdown.SetValueWithoutNotify(_currentType);

        _variableBoard = variableBoard;

        for (int i = 0; i < _variableInputs.Length; i++)
        {
            _variableInputs[i].Disable();
        }
        _variableInputs[_currentType].Enable();
        _variableInputs[_currentType].SetValue(value);

        _vs = NodeBoard.Instance.TargetVisualScripting;
    }

    private void OnTypeChanged(int index)
    {
        _currentType = index;
        for (int i = 0; i < _variableInputs.Length; i++)
        {
            _variableInputs[i].Disable();
        }
        _variableInputs[_currentType].Enable();

        UpdateVariable();
    }

    private void UpdateVariable()
    {
        if (GetDataType() != DataType.List)
        {
            VariableInput input = _variableInputs[_currentType];
            _variableBoard.UpdateVariable(_nameInputField.text, GetDataType(), input.GetValue());
        }
        else
        {
            UpdateListVariable();
        }
    }

    // private void UpdateListVariable()
    // {
    //     VariableInput input = _variableInputs[_currentType];
    //     _variableBoard.UpdateVariable(_nameInputField.text, GetDataType(), input.GetValue());
    // }

    public void UpdateListVariable()
    {
        _vs.UpdateListVariable(_nameInputField.text);
    }

    public void InsertListItem(object value)
    {
        _vs.InsertListItem(_nameInputField.text, value);
    }

    public void UpdateListItem(int index, object value)
    {
        _vs.UpdateListItem(_nameInputField.text, index, value);
    }

    public void RemoveListItem(int index)
    {
        _vs.RemoveListItem(_nameInputField.text, index);
    }

    private DataType GetDataType()
    {
        return _typeDropdown.value switch
        {
            _stringTypeIndex => DataType.String,
            _numberTypeIndex => DataType.Number,
            _booleanTypeIndex => DataType.Boolean,
            _vectorTypeIndex => DataType.Vector,
            _listTypeIndex => DataType.List,
            _ => DataType.Any,
        };
    }

    private int GetDataTypeIndex(DataType type)
    {
        return type switch
        {
            DataType.String => _stringTypeIndex,
            DataType.Number => _numberTypeIndex,
            DataType.Boolean => _booleanTypeIndex,
            DataType.Vector => _vectorTypeIndex,
            DataType.List => _listTypeIndex,
            _ => 0,
        };
    }

    private void OnRemove()
    {
        _variableBoard.RemoveVariable(_nameInputField.text, this);
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