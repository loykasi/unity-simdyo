using TMPro;
using UnityEngine;

public class NodeMenuItem : MonoBehaviour
{
    public RectTransform RectTransform;

    public NodeMenu NodeMenu { get; set; }
    public ScriptNodeData NodeData
    {
        get => _nodeData;
        set
        {
            _nodeData = value;
            UpdateItem();
        }
    }
    private ScriptNodeData _nodeData;

    [SerializeField] private TMP_Text _textField;

    private void UpdateItem()
    {
        _textField.SetText(_nodeData.Title);
    }

    public void Add()
    {
        NodeMenu.AddNode(_nodeData);
    }
}