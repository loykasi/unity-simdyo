using System.Collections.Generic;
using Loykas.Scripting;
using UnityEngine;
using UnityEngine.UI;

public class NodeList : MonoBehaviour
{
    public ScriptFlowGraph FlowGraph { get; set; }

    [SerializeField] private RectTransform _contentRect;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private NodeListCategoryItem _categoryPrefab;
    
    private Dictionary<ScriptNodeCategory, NodeListCategoryItem> _categories = new();
    private List<ScriptNode> _nodes = new();

    public void Init()
    {
        ScriptNodeFactory.Instance.GetNodes(_nodes, FlowGraph.Flow.IsGlobal);
        UpdateElement();
        _scrollRect.verticalNormalizedPosition = 1f;
    }
    
    private void UpdateElement()
    {
        foreach (NodeListCategoryItem categoryElement in _categories.Values)
        {
            Destroy(categoryElement.gameObject);
        }
        _categories.Clear();

        foreach (var node in _nodes)
        {
            ScriptNodeCategory category = node.Category;
            if (!_categories.TryGetValue(category, out NodeListCategoryItem item))
            {
                item = Instantiate(_categoryPrefab, _contentRect);
                item.Init(category.ToString());
                _categories.Add(category, item);
            }

            item.AddItem(node);
        }
    }
}