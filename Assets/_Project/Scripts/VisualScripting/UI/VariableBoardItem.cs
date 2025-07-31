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

    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private TMP_Dropdown _typeDropdown;

    [SerializeField] private TMP_InputField _valueInputField;
    [SerializeField] private Toggle _valueToggleField;

    private VariableBoard _variableBoard;
    private object _value;

    private void Awake()
    {
        _typeDropdown.onValueChanged.AddListener(OnTypeChanged);
        _valueInputField.onEndEdit.AddListener(OnValueChanged);
        _valueToggleField.onValueChanged.AddListener(OnValueToggleChanged);
    }

    public void Init(string name, VariableBoard variableBoard)
    {
        _nameInputField.text = name;
        _typeDropdown.value = _stringTypeIndex;
        _value = "";

        _variableBoard = variableBoard;
        OnTypeChanged(_stringTypeIndex);
    }

    public void Init(string name, DataType type, object value, VariableBoard variableBoard)
    {
        _nameInputField.text = name;
        _typeDropdown.SetValueWithoutNotify(GetDataTypeIndex(type));
        _value = value;

        _variableBoard = variableBoard;
        
        switch (GetDataTypeIndex(type))
        {
            case _stringTypeIndex:
                _valueInputField.gameObject.SetActive(true);
                _valueToggleField.gameObject.SetActive(false);
                _valueInputField.text = _value.ToString();
                break;
            case _numberTypeIndex:
                _valueInputField.gameObject.SetActive(true);
                _valueToggleField.gameObject.SetActive(false);
                _valueInputField.text = _value.ToString();
                break;
            case _booleanTypeIndex:
                _valueInputField.gameObject.SetActive(false);
                _valueToggleField.gameObject.SetActive(true);
                _valueToggleField.isOn = (bool)_value;
                break;
        }
    }

    private void OnTypeChanged(int index)
    {
        switch (index)
        {
            case _stringTypeIndex:
                _valueInputField.gameObject.SetActive(true);
                _valueToggleField.gameObject.SetActive(false);
                _valueInputField.text = "";
                _value = "";
                break;
            case _numberTypeIndex:
                _valueInputField.gameObject.SetActive(true);
                _valueToggleField.gameObject.SetActive(false);
                _valueInputField.text = "0.0";
                _value = 0.0;
                break;
            case _booleanTypeIndex:
                _valueInputField.gameObject.SetActive(false);
                _valueToggleField.gameObject.SetActive(true);
                _valueToggleField.isOn = false;
                _value = false;
                break;
        }

        UpdateVariable();
    }

    private void OnValueToggleChanged(bool value)
    {
        if (_typeDropdown.value != _booleanTypeIndex)
        {
            return;
        }

        _value = value;
        UpdateVariable();
    }

    private void OnValueChanged(string value)
    {
        if (_typeDropdown.value == _booleanTypeIndex)
        {
            return;
        }

        if (_typeDropdown.value == _numberTypeIndex)
        {
            if (double.TryParse(value, out double result))
            {
                _valueInputField.SetTextWithoutNotify(result.ToString());
                _value = result;
            }
            else
            {
                _valueInputField.SetTextWithoutNotify("0");
                _value = 0.0;
            }
        }
        else
        {
            _value = value;
        }

        UpdateVariable();
    }

    private void UpdateVariable()
    {
        _variableBoard.UpdateVariable(_nameInputField.text, GetDataType(), _value);
    }

    private DataType GetDataType()
    {
        return _typeDropdown.value switch
        {
            _stringTypeIndex => DataType.String,
            _numberTypeIndex => DataType.Number,
            _booleanTypeIndex => DataType.Boolean,
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
            _ => 0,
        };
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