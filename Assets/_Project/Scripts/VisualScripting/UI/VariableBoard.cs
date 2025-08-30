using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VariableBoard : MonoBehaviour
{
    [SerializeField] private VariableBoardItem _itemPrefab;
    [SerializeField] private Transform _contentHolder;

    [Space]
    [SerializeField] private TMP_InputField _nameInputField;

    private List<VariableBoardItem> _variableItems = new();

    public void Init()
    {
        Clear();
        Load();
    }

    private void Clear()
    {
        for (int i = 0; i < _variableItems.Count; i++)
        {
            Destroy(_variableItems[i].gameObject);
        }

        _variableItems.Clear();
    }

    private void Load()
    {
        VisualScripting vs = NodeBoard.Instance.TargetVisualScripting;

        foreach (var item in vs.Variables)
        {
            string name = item.Key;
            AddVariableItem(name, item.Value);
        }
    }

    public void AddVariable()
    {
        VisualScripting vs = NodeBoard.Instance.TargetVisualScripting;
        if (vs != null)
        {
            string name = _nameInputField.text;
            if (vs.AddVariable(name))
            {
                VariableBoardItem item = Instantiate(_itemPrefab, _contentHolder);
                item.Init(name, this);
                _variableItems.Add(item);
                _nameInputField.text = string.Empty;
            }
        }
    }

    private void AddVariableItem(string name, Variable variable)
    {
        VariableBoardItem item = Instantiate(_itemPrefab, _contentHolder);
        item.Init(name, variable.Type, variable.SubType, variable.Value, this);
        _variableItems.Add(item);
    }

    public void UpdateVariable(string name, DataType type, object value)
    {
        VisualScripting vs = NodeBoard.Instance.TargetVisualScripting;
        vs.UpdateVariable(name, type, value);
    }

    public void RemoveVariable(string name, VariableBoardItem variableItem)
    {
        VisualScripting vs = NodeBoard.Instance.TargetVisualScripting;
        if (vs.RemoveVariable(name))
        {
            _variableItems.Remove(variableItem);
            Destroy(variableItem.gameObject);
        }
    }
}