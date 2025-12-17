using System;
using System.Collections.Generic;
using Loykas.Scripting;
using UnityEngine;
using UnityEngine.UI;

public class FunctionMenu : MonoBehaviour
{
    [Header("Modal")]
    [SerializeField] private GameObject _window;
    [SerializeField] private Button _closeButton;

    [Header("Input")]
    [SerializeField] private StringInput _nameInput;
    [SerializeField] private Button _addInputButton;
    [SerializeField] private Transform _inputHolder;
    [SerializeField] private FunctionMenuInput _functionInputPrefab;

    private List<FunctionMenuInput> _inputs = new();

    private ScriptFunction _function;

    private readonly string _inputBaseName = "newInput";
    private List<string> _inputNames = new();    // For generate new unique name

    private void Awake()
    {
        _closeButton.onClick.AddListener(Close);
        _addInputButton.onClick.AddListener(AddInput);
    }

    private void OnEnable()
    {
        FunctionBoard.OnSelectItem += OnSelectItem;

        _nameInput.OnSubmit += UpdateName;
    }

    private void OnDisable()
    {
        FunctionBoard.OnSelectItem -= OnSelectItem;

        _nameInput.OnSubmit -= UpdateName;
    }

    private void OnSelectItem(FunctionBoardItem item)
    {
        _window.SetActive(true);
        if (_function != null)
        {
            _function.OnUpdated -= OnFunctionUpdate;
        }

        _function = item.Function;
        _function.OnUpdated += OnFunctionUpdate;

        //_nameInput.SetValue(item.Function.Name);
        OnFunctionUpdate();
    }

    public void Close()
    {
        _window.SetActive(false);
    }

    private void OnFunctionUpdate()
    {
        _nameInput.SetValue(_function.Name);

        if (_function.Inputs.Count < _inputs.Count)
        {
            int deleteFrom = _inputs.Count - 1;
            int deleteTo = _function.Inputs.Count - 1;

            for (int i = deleteFrom; i > deleteTo; i--)
            {
                Destroy(_inputs[i].gameObject);
                _inputs.RemoveAt(i);
            }
        }

        for (int i = 0; i < _function.Inputs.Count; i++)
        {
            FunctionInput functionInput = _function.Inputs[i];

            UpdateUIInput(i, functionInput);
        }
    }

    private void UpdateUIInput(int index, FunctionInput functionInput)
    {
        FunctionMenuInput inputElement;
        if (index >= _inputs.Count)
        {
            inputElement = Instantiate(_functionInputPrefab, _inputHolder);
            _inputs.Add(inputElement);
        }
        else
        {
            inputElement = _inputs[index];
        }
        
        inputElement.Init(this, functionInput.Name, functionInput.Type);
    }

    private void UpdateName(object value)
    {
        _function.EditName((string)value);
    }

    public void AddInput()
    {
        string inputName = GetNewInputName();
        _function.AddInput(inputName);
    }

    private string GetNewInputName()
    {
        _inputNames.Clear();
        for (int i = 0; i < _inputs.Count; i++)
        {
            _inputNames.Add(_inputs[i].Name);
        }
        return Utils.GenerateUniqueName(_inputBaseName, _inputNames);
    }

    public void UpdateInput(FunctionMenuInput input)
    {
        int index = _inputs.FindIndex(i => i == input);

        _function.EditInput(index, input.Name, input.Type);
    }

    public void DeleteInput(FunctionMenuInput input)
    {
        int index = _inputs.FindIndex(i => i == input);
        if (index == -1)
        {
            return;
        }

        _function.DeleteInput(index);
    }
}