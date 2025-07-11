using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UINode : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public class UINodeLine
    {
        public UINodeLine(int index, Vector3 offset, UILineRenderer lineRenderer)
        {
            Index = index;
            Offset = offset;
            LineRenderer = lineRenderer;
        }

        public int Index;
        public Vector3 Offset;
        public UILineRenderer LineRenderer;
    }

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

    private List<UINodePort> _ports = new();
    private List<UINodeLine> _connectionLines = new();

    [SerializeField] private TMP_Text _nodeTitle;

    [SerializeField] private Transform _inputHolder;
    [SerializeField] private Transform _outputHolder;

    [SerializeField] private UINodePort _inputTriggerPrefab;
    [SerializeField] private UINodePort _inputValuePrefab;
    [SerializeField] private UINodePort _outputTriggerPrefab;
    [SerializeField] private UINodePort _outputValuePrefab;

    private Vector2 _offsetFromMouse;

    private void UpdateNodeUI()
    {
        if (Node == null) return;

        _nodeTitle.SetText(Node.Title);

        for (int i = 0; i < Node.InputTriggers.Count; i++)
        {
            UINodePort port = Instantiate(_inputTriggerPrefab, _inputHolder);
            port.UINode = this;
            port.Port = Node.InputTriggers[i];
            port.Init();
            _ports.Add(port);
        }

        for (int i = 0; i < Node.OutputTriggers.Count; i++)
        {
            UINodePort port = Instantiate(_outputTriggerPrefab, _outputHolder);
            port.UINode = this;
            port.Port = Node.OutputTriggers[i];
            port.Init();
            _ports.Add(port);
        }

        for (int i = 0; i < Node.ValueInputs.Count; i++)
        {
            UINodePort port = Instantiate(_inputValuePrefab, _inputHolder);
            port.UINode = this;
            port.Port = Node.ValueInputs[i];
            port.Init();
            _ports.Add(port);
        }

        for (int i = 0; i < Node.ValueOutputs.Count; i++)
        {
            UINodePort port = Instantiate(_outputValuePrefab, _outputHolder);
            port.UINode = this;
            port.Port = Node.ValueOutputs[i];
            port.Init();
            _ports.Add(port);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _offsetFromMouse = Mouse.current.position.ReadValue() - new Vector2(transform.position.x, transform.position.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Mouse.current.position.ReadValue() - _offsetFromMouse;

        DragConnectionLines();
    }

    private void DragConnectionLines()
    {
        for (int i = 0; i < _connectionLines.Count; i++)
        {
            int index = _connectionLines[i].Index;
            Vector3 offset = _connectionLines[i].Offset;
            _connectionLines[i].LineRenderer.Points[index] = transform.localPosition - offset;
            _connectionLines[i].LineRenderer.UpdateVertex();
        }
    }

    public void AddConnectionLine(UILineRenderer lineRenderer, Vector3 position, bool isHead)
    {
        Vector3 offset = transform.localPosition - position;
        int index = isHead ? 0 : 1;
        _connectionLines.Add(new UINodeLine(index, offset, lineRenderer));
    }
}
