using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NodeBoard : Singleton<NodeBoard>, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ScriptFlowGraph FlowGraph { get; set; }
    public ScriptFlow Flow => FlowGraph.Flow;

    [SerializeField] private RectTransform _holder;
    private Vector2 _offsetFromMouse;

    [Space]
    [SerializeField] private UINode _nodePrefab;

    [Space]
    [SerializeField] private NodeConnectionPreview _nodeConnectionPreview;
    [SerializeField] private float _sizePadding;

    [Space]
    [SerializeField] private NodeMenu _nodeMenu;

    private bool _hasPort;

    private UINodePort _fromUIPort;
    private ScriptNode _fromNode;
    private IPort _fromPort;

    private UINodePort _toUIPort;
    private ScriptNode _toNode;
    private IPort _toPort;

    private IGraphElement _selectedElement;

    private Vector2 _previousMousePosition;

    private List<UINode> _nodes = new();
    private List<GameObject> _lineObjects = new();

    private bool _isHover = false;
    private Vector3 _openMenuPosition;

    public void Init()
    {
        ClearBoard();

        Flow.OnNodeAdded += OnNodeAdded;

        LoadBoard();
    }

    public void Close()
    {
        if (Flow != null)
        {
            Flow.OnNodeAdded -= OnNodeAdded;
        }
    }

    private void ClearBoard()
    {
        for (int i = 0; i < _nodes.Count; i++)
        {
            if (_nodes[i] != null)
            {
                Destroy(_nodes[i].gameObject);
            }
            else
            {
                Debug.Log(_nodes[i]);
            }
        }
        for (int i = 0; i < _lineObjects.Count; i++)
        {
            Destroy(_lineObjects[i]);
        }

        _nodes.Clear();
        _lineObjects.Clear();
    }

    private void LoadBoard()
    {
        List<ScriptNode> nodes = Flow.Nodes;
        List<NodeConnection> connections = Flow.Connections;
        for (int i = 0; i < nodes.Count; i++)
        {
            AddNodeToBoard(nodes[i]);
        }
        for (int i = 0; i < connections.Count; i++)
        {
            NodeConnection connection = connections[i];
            
            UINode source = _nodes.Find((node) => node.Node == connection.Source.Node);
            UINode destination = _nodes.Find((node) => node.Node == connection.Destination.Node);
            UINodePort sourcePort = source.Ports.Find((port) => port.Port == connection.Source);
            UINodePort destinationPort = destination.Ports.Find((port) => port.Port == connection.Destination);

            // Debug.Log($"{sourcePort} | {connection.Source} | {source.Node} | {destinationPort} | {connection.Destination} | {destination.Node}");
            if (connection.Source == null || connection.Destination == null || source == null || destination == null || sourcePort == null || destinationPort == null)
            {
                Debug.Log($"{source} | {connection.Source} | {source.Node} | {destinationPort} | {connection.Destination} | {destination.Node}");
                return;
            }
            Connect(sourcePort, connection.Source, source.Node, destinationPort, connection.Destination, destination.Node);
        }
    }

    private void OnNodeAdded(ScriptNode scriptNode)
    {
        UINode node = Instantiate(_nodePrefab, _holder);
     
        node.Node = scriptNode;
        node.transform.position = _openMenuPosition;
        scriptNode.Positon = _openMenuPosition;

        _nodes.Add(node);
    }

    private void AddNodeToBoard(ScriptNode scriptNode)
    {
        UINode node = Instantiate(_nodePrefab, _holder);
     
        node.Node = scriptNode;

        _nodes.Add(node);
    }

    public void AddNode(ScriptNodeData nodeData)
    {
        Flow.AddNode(nodeData);
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

        if (Mouse.current.rightButton.wasPressedThisFrame && _isHover)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            _nodeMenu.Open(mousePosition);

            _openMenuPosition = mousePosition;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _offsetFromMouse = Mouse.current.position.ReadValue() - new Vector2(_holder.position.x, _holder.position.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _holder.position = Mouse.current.position.ReadValue() - _offsetFromMouse;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHover = false;
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
        if (!_fromPort.CanConnect(port) || !port.CanConnect(_fromPort)) return;

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

    private void Connect(UINodePort fromUIPort, IPort fromPort, ScriptNode fromNode, UINodePort toUIPort, IPort toPort, ScriptNode toNode)
    {
        _fromUIPort = fromUIPort;
        _fromPort = fromPort;
        _fromNode = fromNode;
        _toUIPort = toUIPort;
        _toPort = toPort;
        _toNode = toNode;

        AddConnectionLine();
        _fromUIPort = null;
        _fromPort = null;
        _fromNode = null;
        _toUIPort = null;
        _toPort = null;
        _toNode = null;
    }

    private void TryConnect()
    {
        if (!_hasPort) return;

        if (_fromUIPort.Edge == NodePortEdge.Right)
        {
            if (Flow.TryConnect(_fromNode, _fromPort, _toNode, _toPort))
            {
                AddConnectionLine();

                AfterAdd();
            }
        }
        else
        {
            if (Flow.TryConnect(_toNode, _toPort, _fromNode, _fromPort))
            {
                AddConnectionLine();

                AfterAdd();
            }
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
            if (_fromPort is InputValue fromPort)
            {
                _fromUIPort.ValidConnection(fromPort.Source);
            }

            if (_toPort is InputValue toPort)
            {
                _toUIPort.ValidConnection(toPort.Source);
            }
        }

        _fromUIPort.AfterAdd();
        _toUIPort.AfterAdd();
    }

    private void AddConnectionLine()
    {
        GameObject lineObject = new("line");
        _lineObjects.Add(lineObject);

        lineObject.AddComponent<CanvasRenderer>();
        UILineConnection lineConnection = lineObject.AddComponent<UILineConnection>();
        RectTransform rect = lineObject.AddComponent<RectTransform>();

        lineObject.transform.SetParent(_holder);
        rect.anchoredPosition = Vector2.zero;

        UILineRenderer lineRenderer = lineObject.AddComponent<UILineRenderer>();
        lineRenderer.Rect = rect;
        lineConnection.LineRenderer = lineRenderer;
        if (_fromUIPort.Edge == NodePortEdge.Right)
        {
            lineConnection.Source = _fromUIPort;
            lineConnection.Destination = _toUIPort;
        }
        else
        {
            lineConnection.Source = _toUIPort;
            lineConnection.Destination = _fromUIPort;
        }

        lineRenderer.Init(4);
        lineRenderer.Thickness = 3;
        lineRenderer.CornerRadius = 30;
        lineRenderer.CornerSegment = 5;

        // calculate bound;
        Vector3 headPosition;
        Vector3 tailPosition;

        if (_fromUIPort.Edge == NodePortEdge.Right)
        {
            headPosition = _fromUIPort.HandlePosition;
            tailPosition = _toUIPort.HandlePosition;
        }
        else
        {
            headPosition = _toUIPort.HandlePosition;
            tailPosition = _fromUIPort.HandlePosition;
        }

        // Debug.Log($"{headPosition} | {tailPosition}");

        Vector3 size = headPosition - tailPosition;
        Vector3 center = (headPosition + tailPosition) / 2.0f;

        lineRenderer.Points[0] = headPosition - center;
        lineRenderer.Points[1] = headPosition + Vector3.right * 50f - center;
        lineRenderer.Points[2] = tailPosition + Vector3.left * 50f - center;
        lineRenderer.Points[3] = tailPosition - center;
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