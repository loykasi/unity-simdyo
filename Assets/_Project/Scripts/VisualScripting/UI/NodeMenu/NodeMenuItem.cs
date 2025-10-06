using TMPro;
using UnityEngine;

namespace Loykas.Scripting
{
    public class NodeMenuItem : MonoBehaviour
    {
        public RectTransform RectTransform;

        public NodeMenu NodeMenu { get; set; }
        public ScriptNodeContent NodeData
        {
            get => _nodeData;
            set
            {
                _nodeData = value;
                UpdateItem();
            }
        }
        private ScriptNodeContent _nodeData;

        [SerializeField] private TMP_Text _textField;

        private void UpdateItem()
        {
            _textField.SetText(_nodeData.Type.Name);
        }

        public void Add()
        {
            NodeMenu.AddNode(_nodeData);
        }
    }
}