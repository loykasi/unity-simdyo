using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UINode : MonoBehaviour, IDragHandler, IBeginDragHandler, IGraphElement, IPointerEnterHandler, IPointerExitHandler
{
    public ScriptNode Node
    {
        get => _node;
        set
        {
            _node = value;
            UpdateNodeUI();
        }
    }

    private ScriptNode _node;

    [HideInInspector] public List<UINodePort> Ports = new();
    [HideInInspector] public List<UINodePort> InputPorts = new();
    [HideInInspector] public List<UINodePort> OutputPorts = new();

    [Header("References")]
    [SerializeField] private TMP_Text _nodeTitle;
    [SerializeField] private RectTransform _selectedBorder;
    [SerializeField] private float _borderSize;

    [Header("Node Holders")]
    [SerializeField] private RectTransform _inputHolder;
    [SerializeField] private RectTransform _outputHolder;

    [Header("Head and body")]
    [SerializeField] private RectTransform _head;
    [SerializeField] private RectTransform _body;

    [Header("Prefabs")]
    [SerializeField] private UINodePort _inputTriggerPrefab;
    [SerializeField] private UINodePort _inputValuePrefab;
    [SerializeField] private UINodePort _outputTriggerPrefab;
    [SerializeField] private UINodePort _outputValuePrefab;

    private Vector2 _offsetFromMouse;
    // private bool _isMouseOver = false;

    private readonly float _inputOutputDistance = 20f;
    private readonly float _minWidth = 200f;
    private readonly float _topBottomPadding = 10f;

    private void UpdateNodeUI()
    {
        if (Node == null) return;

        transform.localPosition = Node.Positon;
        _nodeTitle.SetText(Node.Title);

        for (int i = 0; i < Node.InputTriggers.Count; i++)
        {
            UINodePort port = Instantiate(_inputTriggerPrefab, _inputHolder);
            port.UINode = this;
            port.Port = Node.InputTriggers[i];
            port.Init();
            Ports.Add(port);
            InputPorts.Add(port);
        }

        for (int i = 0; i < Node.OutputTriggers.Count; i++)
        {
            UINodePort port = Instantiate(_outputTriggerPrefab, _outputHolder);
            port.UINode = this;
            port.Port = Node.OutputTriggers[i];
            port.Init();
            Ports.Add(port);
            OutputPorts.Add(port);
        }

        for (int i = 0; i < Node.ValueInputs.Count; i++)
        {
            UINodePort port = Instantiate(_inputValuePrefab, _inputHolder);
            port.UINode = this;
            port.Port = Node.ValueInputs[i];
            port.Init();
            Ports.Add(port);
            InputPorts.Add(port);
        }

        for (int i = 0; i < Node.ValueOutputs.Count; i++)
        {
            UINodePort port = Instantiate(_outputValuePrefab, _outputHolder);
            port.UINode = this;
            port.Port = Node.ValueOutputs[i];
            port.Init();
            Ports.Add(port);
            OutputPorts.Add(port);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_inputHolder);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_outputHolder);

        UpdateSize();
    }

    public void UpdateSize()
    {
        Vector2 inputSize = GetPortGroupMaxSize(InputPorts);
        Vector2 outputSize = GetPortGroupMaxSize(OutputPorts);
        float bodyHeight = (InputPorts.Count > OutputPorts.Count ? inputSize.y : outputSize.y) + _topBottomPadding;

        float x = inputSize.x + outputSize.x + _inputOutputDistance;
        x = Mathf.Max(x, _minWidth);

        _head.sizeDelta = new Vector2(x, _head.sizeDelta.y);
        _body.sizeDelta = new Vector2(x, bodyHeight);

        UpdateBorder();
    }

    private void UpdateBorder()
    {
        Vector2 nodeSize = new
        (
            _head.sizeDelta.x,
            _head.sizeDelta.y + _body.sizeDelta.y
        );

        Vector2 borderSize = nodeSize + _borderSize * 2 * Vector2.one;
        _selectedBorder.sizeDelta = borderSize;
    }

    private Vector2 GetPortGroupMaxSize(List<UINodePort> ports)
    {
        if (ports.Count == 0)
        {
            return Vector2.zero;
        }

        float y = ports.Count * 30f;
        float x = ports[0].Rect.sizeDelta.x;
        for (int i = 1; i < ports.Count; i++)
        {
            float value = ports[i].Rect.sizeDelta.x;
            if (value > x)
            {
                x = value;
            }
        }

        return new Vector2(x, y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _offsetFromMouse = Mouse.current.position.ReadValue() - new Vector2(transform.position.x, transform.position.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Mouse.current.position.ReadValue() - _offsetFromMouse;
        Node.Positon = transform.localPosition;

        for (int i = 0; i < Ports.Count; i++)
        {
            Ports[i].UpdateLines();
        }
    }

    public void Select()
    {
        _selectedBorder.gameObject.SetActive(true);
    }

    public void Delete()
    {
        for (int i = 0; i < Ports.Count; i++)
        {
            Ports[i].DeleteAllLines();
        }
        Destroy(gameObject);
    }

    public void Unselect()
    {
        _selectedBorder.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // _isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // _isMouseOver = false;
    }
}
