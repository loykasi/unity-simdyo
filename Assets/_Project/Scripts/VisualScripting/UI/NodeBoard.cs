using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NodeBoard : Singleton<NodeBoard>, IBeginDragHandler, IDragHandler
{
    public VisualScripting TargetVisualScripting;

    [SerializeField] private RectTransform _holder;
    private Vector2 _offsetFromMouse;

    [Space]
    [SerializeField] private UINode _nodePrefab;

    [Space]
    [SerializeField] private NodeConnectionPreview _nodeConnectionPreview;
    [SerializeField] private float _sizePadding;

    private bool _hasPort;

    private UINodePort _fromUIPort;
    private ScriptNode _fromNode;
    private IPort _fromPort;

    private UINodePort _toUIPort;
    private ScriptNode _toNode;
    private IPort _toPort;

    private IGraphElement _selectedElement;

    private Vector2 _previousMousePosition;

    private void OnEnable()
    {
        TargetVisualScripting.OnNodeAdded += OnNodeAdded;
    }

    private void OnDisable()
    {
        TargetVisualScripting.OnNodeAdded -= OnNodeAdded;
    }

    private void OnNodeAdded(ScriptNode scriptNode)
    {
        UINode node = Instantiate(_nodePrefab, _holder);
        node.Node = scriptNode;
    }

    private void Start()
    {
        LoadBoard();
    }

    public List<RaycastResult> results = new();
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _previousMousePosition = Mouse.current.position.ReadValue();
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (Mouse.current.position.ReadValue() != _previousMousePosition)
            {
                return;
            }

            var data = new PointerEventData(EventSystem.current)
            {
                position = Mouse.current.position.ReadValue()
            };
            EventSystem.current.RaycastAll(data, results);

            if (results.Count == 0)
            {
                _selectedElement?.Unselect();
                return;
            }

            if (results[0].gameObject.TryGetComponent(out IGraphElement element))
            {
                _selectedElement?.Unselect();
                element.Select();
                _selectedElement = element;
                return;
            }

            element = results[0].gameObject.GetComponentInParent<IGraphElement>();
            if (element != null)
            {
                _selectedElement?.Unselect();
                element.Select();
                _selectedElement = element;
                return;
            }

            _selectedElement?.Unselect();
            _selectedElement = null;
        }

        if (Keyboard.current.deleteKey.wasPressedThisFrame)
        {
            _selectedElement?.Delete();
            _selectedElement = null;
        }
    }

    private void LoadBoard()
    {
        // if (TargetVisualScripting.startNode != null)
        // {
        //     UINode node = Instantiate(_nodePrefab, _holder);
        //     node.Node = TargetVisualScripting.startNode;
        // }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _offsetFromMouse = Mouse.current.position.ReadValue() - new Vector2(_holder.position.x, _holder.position.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _holder.position = Mouse.current.position.ReadValue() - _offsetFromMouse;
    }

    public void StartPreviewConnect(UINodePort fromPort, Vector3 startPosition, NodePortEdge edge)
    {
        _fromUIPort = fromPort;
        _fromNode = fromPort.UINode.Node;
        _fromPort = fromPort.Port;

        _nodeConnectionPreview.StartPreviewConnect(startPosition, edge);
    }

    public void DragPreviewConnect(Vector3 mousePosition)
    {
        _nodeConnectionPreview.DragPreviewConnect(mousePosition);
    }

    public void EndPreviewConnect()
    {
        _nodeConnectionPreview.EndPreviewConnect();

        TryConnect();
        _fromUIPort = null;
        _fromPort = null;
        _fromNode = null;
        _toUIPort = null;
        _toPort = null;
        _toNode = null;
    }

    public void OnEnterPort(UINodePort toPort, Vector3 position)
    {
        IPort port = toPort.Port;
        if (_fromPort == null) return;
        if (!_fromPort.CanConnectTo(port)) return;

        _toUIPort = toPort;
        _toNode = toPort.UINode.Node;
        _toPort = port;
        _hasPort = true;

        _nodeConnectionPreview.EnterPort(position);
    }

    public void OnExitPort()
    {
        _hasPort = false;
        _nodeConnectionPreview.ExitPort();
    }

    private void TryConnect()
    {
        if (!_hasPort) return;

        if (TargetVisualScripting.TryConnect(_fromNode, _fromPort, _toNode, _toPort))
        {
            AddConnectionLine();

            AfterAdd();
        }
    }

    private void AfterAdd()
    {
        {
            if (_fromPort is OutputTrigger fromPort)
            {
                _fromUIPort.ValidConnection(fromPort.Destination);
            }

            if (_toPort is OutputTrigger toPort)
            {
                _toUIPort.ValidConnection(toPort.Destination);
            }
        }
        {
            if (_fromPort is ValueInput fromPort)
            {
                _fromUIPort.ValidConnection(fromPort.Source);
            }

            if (_toPort is ValueInput toPort)
            {
                _toUIPort.ValidConnection(toPort.Source);
            }
        }
    }

    private void AddConnectionLine()
    {
        GameObject lineObject = new("line");

        lineObject.AddComponent<CanvasRenderer>();
        UILineConnection lineConnection = lineObject.AddComponent<UILineConnection>();
        RectTransform rect = lineObject.AddComponent<RectTransform>();

        lineObject.transform.SetParent(_holder);
        rect.anchoredPosition = Vector2.zero;

        UILineRenderer lineRenderer = lineObject.AddComponent<UILineRenderer>();
        lineRenderer.Rect = rect;
        lineConnection.LineRenderer = lineRenderer;
        lineConnection.Source = _fromUIPort;
        lineConnection.Destination = _toUIPort;

        lineRenderer.Init(4);
        lineRenderer.Thickness = 3;
        lineRenderer.CornerRadius = 30;
        lineRenderer.CornerSegment = 5;

        // calculate bound;
        UILineRenderer previewLineRender = _nodeConnectionPreview.LineRenderer;
        Vector3 size = previewLineRender.Points[3] - previewLineRender.Points[0];
        Vector3 center = (previewLineRender.Points[0] + previewLineRender.Points[3]) / 2.0f;

        lineRenderer.Points[0] = previewLineRender.Points[0] - center;
        lineRenderer.Points[1] = previewLineRender.Points[1] - center;
        lineRenderer.Points[2] = previewLineRender.Points[2] - center;
        lineRenderer.Points[3] = previewLineRender.Points[3] - center;
        rect.localPosition = center - _holder.position;
        rect.sizeDelta = new Vector2(Mathf.Abs(size.x) + _sizePadding, Mathf.Abs(size.y) + _sizePadding);

        _fromUIPort.AddConnection(lineConnection);
        _toUIPort.AddConnection(lineConnection);
    }

    public void UpdateLines(UILineRenderer line, NodePortEdge edge, Vector3 portPosition)
    {
        Vector3 position = portPosition - line.transform.position;

        switch (edge)
        {
            case NodePortEdge.Left:
                line.Points[2] = position + Vector3.left * 50f;
                line.Points[3] = position;
                break;
            case NodePortEdge.Right:
                line.Points[0] = position;
                line.Points[1] = position + Vector3.right * 50f;
                break;
        }

        RecalculateLineBound(line);
        line.UpdateVertex();
    }

    public void RecalculateLineBound(UILineRenderer lineRenderer)
    {
        Vector3 center = lineRenderer.transform.position;
        Vector3 newCenter = (lineRenderer.Points[0] + lineRenderer.Points[3]) / 2.0f + lineRenderer.transform.position;
        Vector2 size = lineRenderer.Points[3] - lineRenderer.Points[0];
        size = new(Mathf.Abs(size.x), Mathf.Abs(size.y));
        Vector3 offset = center - newCenter;

        for (int i = 0; i < lineRenderer.Points.Length; i++)
        {
            lineRenderer.Points[i] += offset;
        }

        lineRenderer.Rect.sizeDelta = size;
        lineRenderer.Rect.position = newCenter;
    }
}