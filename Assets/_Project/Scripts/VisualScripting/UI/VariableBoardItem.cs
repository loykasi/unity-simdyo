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

    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private TMP_Dropdown _typeDropdown;
    [SerializeField] private Button _removeButton;

    [SerializeField] private VariableInput[] _variableInputs;

    private int _currentType;
    private VariableBoard _variableBoard;

    private void Awake()
    {
        _typeDropdown.onValueChanged.AddListener(OnTypeChanged);
        _removeButton.onClick.AddListener(OnRemove);

        foreach (var item in _variableInputs)
        {
            item.OnValueUpdated += UpdateVariable;
        }
    }

    public void Init(string name, VariableBoard variableBoard)
    {
        _nameInputField.text = name;
        _typeDropdown.value = _stringTypeIndex;

        _variableBoard = variableBoard;
        OnTypeChanged(_stringTypeIndex);
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
        VariableInput input = _variableInputs[_currentType];
        _variableBoard.UpdateVariable(_nameInputField.text, GetDataType(), input.GetValue());
    }

    private DataType GetDataType()
    {
        return _typeDropdown.value switch
        {
            _stringTypeIndex => DataType.String,
            _numberTypeIndex => DataType.Number,
            _booleanTypeIndex => DataType.Boolean,
            _vectorTypeIndex => DataType.Vector,
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