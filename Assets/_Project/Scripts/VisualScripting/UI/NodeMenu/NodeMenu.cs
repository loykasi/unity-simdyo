using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Loykas.Scripting
{
    public class NodeMenu : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private RectTransform _canvas;

        [SerializeField] private RectTransform _contentRect;
        [SerializeField] private NodeMenuCategory _categoryPrefab;

        private Dictionary<ScriptNodeCategory, NodeMenuCategory> _categories = new();

        private Vector2 _offsetFromMouse;
        private bool _isHover = false;

        private NodeBoard _nodeBoard;

        private List<ScriptNode> _nodes = new();

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

        public void AddNode(ScriptNode node)
        {
            Debug.Log($"Add node {node}");
            _nodeBoard.AddNode(node);

            Close();
        }

        public void Open(NodeBoard nodeBoard, Vector3 position, IPort port = null)
        {
            _nodeBoard = nodeBoard;

            if (port == null)
            {
                ScriptNodeFactory.Instance.GetNodes(_nodes, _nodeBoard.Flow.IsGlobal);
            }
            else
            {
                ScriptNodeFactory.Instance.GetNodes(_nodes, port, _nodeBoard.Flow.IsGlobal);                
            }

            UpdateMenuElement();

            gameObject.SetActive(true);
            transform.position = position;
            SetPosition();

            foreach (var item in _categories)
            {
                item.Value.SetOpen(false);
            }
        }

        private void SetPosition()
        {
            Vector3 worldPosition = Mouse.current.position.ReadValue();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, worldPosition, null, out Vector2 point);
            Vector2 position = point + new Vector2(_canvas.sizeDelta.x * 0.5f, _canvas.sizeDelta.y * 0.5f);

            Debug.Log($"screen: {position}");

            float xDiff = position.x + _rect.sizeDelta.x - _canvas.sizeDelta.x;
            float yDiff = _rect.sizeDelta.y - position.y;

            if (xDiff > 0)
            {
                position.x -= xDiff;
            }

            if (yDiff > 0)
            {
                position.y += yDiff;
            }

            Debug.Log($"new position: {position}");

            _rect.anchoredPosition = position;
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