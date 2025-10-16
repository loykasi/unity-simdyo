using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    public class FunctionBoard : MonoBehaviour
    {
        public static event Action<FunctionBoardItem> OnSelectItem;
        public ScriptFlowGraph FlowGraph { get; set; }

        [SerializeField] private RectTransform _rect;
        [SerializeField] private RectTransform _holder;
        [SerializeField] private FunctionBoardItem _functionItemPrefab;

        private List<FunctionBoardItem> _functionsItem = new();

        private FunctionBoardItem _selectedItem;

        public void Init()
        {
            Clear();
            Load();
        }

        private void Clear()
        {
            for (int i = 0; i < _functionsItem.Count; i++)
            {
                Destroy(_functionsItem[i].gameObject);
            }
            _functionsItem.Clear();
        }
        
        private void Load()
        {
            ScriptFlow flow = FlowGraph.Flow;

            for (int i = 0; i < flow.Functions.Count; i++)
            {
                AddFunctionItem(flow.Functions[i]);
            }
        }

        public void AddFunction()
        {
            ScriptFunction function = FlowGraph.Flow.AddFunction();
            AddFunctionItem(function);
        }
        
        public void AddFunctionItem(ScriptFunction function)
        {
            FunctionBoardItem functionItem = Instantiate(_functionItemPrefab, _holder);
            functionItem.Init(this, function);
            _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _rect.sizeDelta.y + 45f);

            _functionsItem.Add(functionItem);

            FlowGraph.RebuildSideBarUI();
        }

        public void Select(FunctionBoardItem item)
        {
            _selectedItem = item;
            OnSelectItem?.Invoke(item);
        }
    }
}