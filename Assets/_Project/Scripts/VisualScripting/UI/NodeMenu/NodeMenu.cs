using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NodeMenu : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private NodeCollectionData _nodeCollection;
    [SerializeField] private NodeCategoryCollection _categoryCollection;
    [SerializeField] private RectTransform _content;
    [SerializeField] private NodeMenuCategory _categoryPrefab;

    private Dictionary<NodeCategoryData, NodeMenuCategory> _categories = new();

    private Vector2 _offsetFromMouse;
    private bool _isHover = false;

    private void Start()
    {
        InitMenu();
    }

    private void InitMenu()
    {
        foreach (var category in _categoryCollection.Categories)
        {
            NodeMenuCategory item = Instantiate(_categoryPrefab, _content);
            item.Init(category.Title, _content, this);

            if (!_categories.ContainsKey(category))
            {
                _categories[category] = item;
            }
        }

        foreach (var node in _nodeCollection.Nodes)
        {
            NodeCategoryData category = node.Category;
            if (category == null)
            {
                continue;
            }
            if (_categories.TryGetValue(category, out NodeMenuCategory item))
            {
                item.AddItem(node);
            }
        }
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !_isHover)
        {
            Close();
        }
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    public void AddNode(ScriptNodeData nodeData)
    {
        NodeBoard.Instance.AddNode(nodeData);
        Close();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _offsetFromMouse = eventData.position - new Vector2(transform.position.x, transform.position.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position - _offsetFromMouse;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHover = false;
    }
}