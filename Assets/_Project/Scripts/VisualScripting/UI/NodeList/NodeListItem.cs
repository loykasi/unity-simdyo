using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Loykas.Scripting
{
    public class NodeListItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform RectTransform;

        public NodeMenu NodeMenu { get; set; }
        public ScriptNode NodeData
        {
            get => _nodeData;
            set
            {
                _nodeData = value;
                UpdateItem();
            }
        }
        private ScriptNode _nodeData;

        [SerializeField] private TMP_Text _textField;

        private void UpdateItem()
        {
            string title = GlobalLocalization.Instance.GetValue(_nodeData.GetNameKey());
            _textField.SetText(title);
        }

        public void Add()
        {
            NodeMenu.AddNode(_nodeData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragPreviewSystem.Instance.BeginDrag(transform.position, _textField.text);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragPreviewSystem.Instance.EndDrag();
        }
    }
}