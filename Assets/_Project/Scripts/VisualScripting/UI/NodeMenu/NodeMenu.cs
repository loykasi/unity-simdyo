using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Loykas.Scripting
{
    public class NodeMenu : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _contentRect;
        [SerializeField] private NodeMenuCategory _categoryPrefab;

        private Dictionary<ScriptNodeCategory, NodeMenuCategory> _categories = new();

        private Vector2 _offsetFromMouse;
        private bool _isHover = false;

        private NodeBoard _nodeBoard;

        private List<ScriptNodeContent> _nodes = new();

        private void UpdateMenuElement()
        {
            foreach (NodeMenuCategory categoryElement in _categories.Values)
            {
                Destroy(categoryElement.gameObject);
            }
            _categories.Clear();

            foreach (var node in _nodes)
            {
                ScriptNodeCategory category = node.Category;
                if (!_categories.TryGetValue(category, out NodeMenuCategory item))
                {
                    item = Instantiate(_categoryPrefab, _contentRect);
                    item.Init(category.ToString(), _contentRect, this);
                    _categories.Add(category, item);
                }

                item.AddItem(node);
            }
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && !_isHover)
            {
                Close();
            }
        }

        public void Open(NodeBoard nodeBoard, Vector3 position)
        {
            _nodeBoard = nodeBoard;

            ScriptNodeFactory.Instance.GetNodes(_nodes, _nodeBoard.Flow.IsGlobal);
            UpdateMenuElement();

            gameObject.SetActive(true);
            transform.position = position;

            foreach (var item in _categories)
            {
                item.Value.SetOpen(false);
            }
        }

        public void Open(NodeBoard nodeBoard, Vector3 position, IPort port)
        {
            _nodeBoard = nodeBoard;

            ScriptNodeFactory.Instance.GetNodes(_nodes, port, _nodeBoard.Flow.IsGlobal);
            UpdateMenuElement();

            gameObject.SetActive(true);
            transform.position = position;

            foreach (var item in _categories)
            {
                item.Value.SetOpen(false);
            }
        }

        public void AddNode(ScriptNodeContent nodeData)
        {
            Debug.Log($"Add node {nodeData}");
            _nodeBoard.AddNode(nodeData.Type);

            Close();
        }

        private void Close()
        {
            _nodeBoard.OnMenuClosed();
            _nodeBoard = null;
            gameObject.SetActive(false);
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
}