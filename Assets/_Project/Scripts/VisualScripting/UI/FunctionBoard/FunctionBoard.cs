using System;
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

        private FunctionBoardItem _selectedItem;

        public void AddFunction()
        {
            ScriptFunction function = FlowGraph.Flow.AddFunction();

            FunctionBoardItem functionItem = Instantiate(_functionItemPrefab, _holder);
            functionItem.Init(this, function);
            _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _rect.sizeDelta.y + 45f);

            FlowGraph.RebuildSideBarUI();
        }

        public void Select(FunctionBoardItem item)
        {
            _selectedItem = item;
            OnSelectItem?.Invoke(item);
        }
    }
}