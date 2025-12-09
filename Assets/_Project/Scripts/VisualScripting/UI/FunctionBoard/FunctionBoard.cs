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
        [SerializeField] private GameObject _noDataText;

        private List<FunctionBoardItem> _functionsItems = new();

        private FunctionBoardItem _selectedItem;
        
        private readonly float _headerHeight = 40f;

        private void OnDisable()
        {
            FlowGraph.Flow.OnFunctionDeleted -= OnFunctionDeleted;
        }

        public void Init()
        {
            Clear();
            Load();
        }

        private void Clear()
        {
            for (int i = 0; i < _functionsItems.Count; i++)
            {
                Destroy(_functionsItems[i].gameObject);
            }
            _functionsItems.Clear();
        }
        
        private void Load()
        {
            ScriptFlow flow = FlowGraph.Flow;
            flow.OnFunctionDeleted += OnFunctionDeleted;

            for (int i = 0; i < flow.Functions.Count; i++)
            {
                AddFunctionItem(flow.Functions[i]);
            }
            CheckForNoData();
        }

        private void OnFunctionDeleted(ScriptFunction function)
        {
            int index = _functionsItems.FindIndex(f => f.Function == function);
            if (index != -1)
            {
                FunctionBoardItem functionItem = _functionsItems[index];
                Destroy(functionItem.gameObject);
                _functionsItems.RemoveAt(index);
                CheckForNoData();

                float bodyHeight = GetBodyHeight();
                _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _headerHeight + bodyHeight);
                FlowGraph.RebuildSideBarUI();
            }
        }

        public void AddFunction()
        {
            ScriptFunction function = FlowGraph.Flow.AddFunction();
            FunctionBoardItem functionItem = AddFunctionItem(function);
            CheckForNoData();

            Select(functionItem);
        }
        
        public FunctionBoardItem AddFunctionItem(ScriptFunction function)
        {
            FunctionBoardItem functionItem = Instantiate(_functionItemPrefab, _holder);
            functionItem.Init(this, function);

            _functionsItems.Add(functionItem);

            float bodyHeight = GetBodyHeight();
            _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _headerHeight + bodyHeight);

            FlowGraph.RebuildSideBarUI();

            return functionItem;
        }

        private float GetBodyHeight()
        {
            if (_functionsItems.Count > 0)
            {
                return _functionsItems.Count * 40f + (_functionsItems.Count - 1) * 5f + 10f;
            }
            return 40f;
        }

        private void CheckForNoData()
        {
            _noDataText.SetActive(_functionsItems.Count == 0);
        }

        public void Select(FunctionBoardItem item)
        {
            _selectedItem = item;
            OnSelectItem?.Invoke(item);
        }
    }
}