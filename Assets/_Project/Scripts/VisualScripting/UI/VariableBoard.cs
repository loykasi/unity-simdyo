using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Loykas.Scripting
{
    public class VariableBoard : MonoBehaviour
    {
        public ScriptFlowGraph FlowGraph { get; set; }

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
            ScriptFlow flow = FlowGraph.Flow;

            foreach (var item in flow.Variables)
            {
                string name = item.Key;
                AddVariableItem(name, item.Value);
            }
        }

        public void AddVariable()
        {
            ScriptFlow flow = FlowGraph.Flow;
            string name = _nameInputField.text;

            if (flow.AddVariable(name))
            {
                VariableBoardItem item = Instantiate(_itemPrefab, _contentHolder);
                item.Init(name, this);
                _variableItems.Add(item);
                _nameInputField.text = string.Empty;
            }
        }

        private void AddVariableItem(string name, Variable variable)
        {
            VariableBoardItem item = Instantiate(_itemPrefab, _contentHolder);
            item.Init(name, variable.Type, variable.Value, this);
            _variableItems.Add(item);
        }

        public void RemoveVariable(string name, VariableBoardItem variableItem)
        {
            if (FlowGraph.Flow.RemoveVariable(name))
            {
                _variableItems.Remove(variableItem);
                Destroy(variableItem.gameObject);
            }
        }
    }
}