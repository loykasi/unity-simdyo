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

    [Header("Input")]
    [SerializeField] private RectTransform _inputHolder;
    [SerializeField] private UIInputData _inputDataReference;
    [SerializeField] private DataTypeController.CustomType _defaultInputType;
    private BaseInput _input;

    private Variable _variable;
    private VariableBoard _variableBoard;

    private readonly float _width = 300f;
    private readonly float _verticalPadding = 10f;
    private readonly float _titleAndTypeHeight = 60f;

    private void Start()
    {
        InitDropDown();

        _typeDropdown.onValueChanged.AddListener(OnTypeChanged);
        _removeButton.onClick.AddListener(OnRemove);
    }

    private void InitDropDown()
    {
        if (_typeDropdown.options.Count != 0)
        {
            return;
        }

        _typeDropdown.AddOptions(DataTypeController.DataTypesDropdownValues);
    }

    public void Init(string name, VariableBoard variableBoard)
    {
        _nameInputField.text = name;
        _typeDropdown.value = 0;

        _variableBoard = variableBoard;
        _variable = NodeBoard.Instance.Flow.GetVariable(name);

        OnTypeChanged(0);
    }

    // NEED TO FIX THIS
    // #: ListType
    public void Init(string name, DataType type, ListType? subType, object value, VariableBoard variableBoard)
    {
        InitDropDown();

        int typeIndex = GetDataTypeIndex(type, subType);

        _nameInputField.text = name;
        _typeDropdown.SetValueWithoutNotify(typeIndex);

        _variableBoard = variableBoard;

        OnTypeChanged(typeIndex);
    }

    private void OnTypeChanged(int index)
    {
        var type = DataTypeController.DataTypeList[index];
        
        ValueHandler.SetDefaultValue(_variable, type.Type);

        if (_input != null)
        {
            Destroy(_input.gameObject);
        }

        _input = _inputDataReference.Get(type.Type, type.SubType);
        _input.Rect.SetParent(_inputHolder, false);
        _input.Enable();

        _input.OnSubmit += OnInputSubmit;
        _input.SetValueInstance(_variable);
    }

    private void OnInputSubmit(object value)
    {
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
            _verticalPadding + _titleAndTypeHeight + _input.Size.y
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