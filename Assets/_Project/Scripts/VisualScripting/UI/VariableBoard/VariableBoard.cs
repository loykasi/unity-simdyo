using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    public class VariableBoard : MonoBehaviour
    {
        public static event Action<ScriptFlow, Variable> OnSelectItem;
        public ScriptFlowGraph FlowGraph { get; set; }

        [SerializeField] private VariableBoardItem _itemPrefab;
        [SerializeField] private Transform _contentHolder;
        [SerializeField] private GameObject _noDataText;

        private List<VariableBoardItem> _variableItems = new();

        private void OnDisable()
        {
            FlowGraph.Flow.OnVariableDeleted -= OnVariableDeleted;
        }

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
            flow.OnVariableDeleted += OnVariableDeleted;

            foreach (var item in flow.Variables.Values)
            {
                AddVariableItem(item);
            }
            CheckForNoData();
        }

        public void AddVariable()
        {
            ScriptFlow flow = FlowGraph.Flow;
            Variable variable = flow.AddVariable();

            AddVariableItem(variable);
            CheckForNoData();
        }

        private void AddVariableItem(Variable variable)
        {
            VariableBoardItem item = Instantiate(_itemPrefab, _contentHolder);
            item.Init(this, variable);
            _variableItems.Add(item);
        }

        private void CheckForNoData()
        {
            _noDataText.SetActive(_variableItems.Count == 0);
        }

        public void Select(VariableBoardItem item)
        {
            OnSelectItem?.Invoke(FlowGraph.Flow, item.Variable);
        }

        private void OnVariableDeleted(Variable variable)
        {
            int index = _variableItems.FindIndex(v => v.Variable == variable);
            if (index != -1)
            {
                VariableBoardItem item = _variableItems[index];
                Destroy(item.gameObject);
                _variableItems.RemoveAt(index);
                CheckForNoData();
            }
            
        }
    }
}