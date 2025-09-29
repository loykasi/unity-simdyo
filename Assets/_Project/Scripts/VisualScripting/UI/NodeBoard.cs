using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Loykas.Scripting
{
    public class NodeBoard : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        public ScriptFlowGraph FlowGraph { get; set; }
        public ScriptFlow Flow => FlowGraph.Flow;
        public SceneEntity Entity => FlowGraph.Entity;

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
        private UINodePort _toUIPort;

        private IGraphElement _selectedElement;

        private Vector2 _previousMousePosition;

        private List<UINode> _nodes = new();
        private List<UILineConnection> _lines = new();

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
            _selectedElement = null;
        }

        private void ClearBoard()
        {
            for (int i = 0; i < _nodes.Count; i++)
            {
                Destroy(_nodes[i].gameObject);
            }
            for (int i = 0; i < _lines.Count; i++)
            {
                Destroy(_lines[i].gameObject);
            }

            _nodes.Clear();
            _lines.Clear();
        }

        private void LoadBoard()
        {
            _holder.localPosition = Flow.Pan;
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

                if (connection.Source == null || connection.Destination == null || source == null || destination == null || sourcePort == null || destinationPort == null)
                {
                    // Debug.Log($"{source} | {connection.Source} | {source.Node} | {destinationPort} | {connection.Destination} | {destination.Node}");
                    return;
                }
                Connect(sourcePort, destinationPort);
            }

            // Debug.Log("======From System");
            // Debug.Log($"Node count: {nodes.Count}");
            // Debug.Log($"Connection count: {connections.Count}");

            // Debug.Log("======From UI");
            // Debug.Log($"Node count: {_nodes.Count}");
            // Debug.Log($"Connection count: {_lines.Count}");
        }

        private void OnNodeAdded(ScriptNode scriptNode)
        {
            UINode node = Instantiate(_nodePrefab, _holder);

            node.Board = this;
            node.Node = scriptNode;
            node.transform.position = _openMenuPosition;
            scriptNode.Position = node.transform.localPosition;

            _nodes.Add(node);
        }

        private void AddNodeToBoard(ScriptNode scriptNode)
        {
            UINode node = Instantiate(_nodePrefab, _holder);

            node.Board = this;
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

            HandleDelete();

            if (Mouse.current.rightButton.wasPressedThisFrame && _isHover)
            {
                Vector3 mousePosition = Mouse.current.position.ReadValue();
                _nodeMenu.Open(this, mousePosition);

                _openMenuPosition = mousePosition;
            }
        }

        private void HandleDelete()
        {
            if (Keyboard.current.deleteKey.wasPressedThisFrame)
            {
                _selectedElement?.Delete();
                _selectedElement = null;
            }
        }

        public void DeleteConnection(UILineConnection lineConnection)
        {
            Flow.Disconnect(lineConnection.Source.Port, lineConnection.Destination.Port);
            DeleteConnectionVisual(lineConnection);
        }

        public void DeleteConnectionVisual(UILineConnection lineConnection)
        {
            lineConnection.DeleteVisual();
            _lines.Remove(lineConnection);
        }

        public void DeleteNode(UINode node)
        {
            for (int i = 0; i < node.Ports.Count; i++)
            {
                node.Ports[i].DeleteAllLines();
            }

            Flow.DeleteNode(node.Node);

            _nodes.Remove(node);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _offsetFromMouse = Mouse.current.position.ReadValue() - new Vector2(_holder.position.x, _holder.position.y);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _holder.position = Mouse.current.position.ReadValue() - _offsetFromMouse;
            Flow.Pan = _holder.localPosition;
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
        }

        public void OnEnterPort(UINodePort toPort, Vector3 position)
        {
            IPort port = toPort.Port;
            if (_fromUIPort == null) return;
            if (!_fromUIPort.Port.CanConnect(port) || !port.CanConnect(_fromUIPort.Port)) return;

            _toUIPort = toPort;
            _hasPort = true;

            _nodeConnectionPreview.EnterPort(position);
        }

        public void OnExitPort()
        {
            _hasPort = false;
            _nodeConnectionPreview.ExitPort();
        }

        private void Connect(UINodePort fromUIPort, UINodePort toUIPort)
        {
            _fromUIPort = fromUIPort;
            _toUIPort = toUIPort;

            AddConnectionLine();
            _fromUIPort = null;
            _toUIPort = null;
        }

        private void TryConnect()
        {
            if (!_hasPort) return;

            SwapPort();

            if (Flow.TryConnect(_fromUIPort.Port, _toUIPort.Port))
            {
                AddConnectionLine();

                AfterAdd();
            }

            _fromUIPort = null;
            _toUIPort = null;
        }

        private void SwapPort()
        {
            if (_fromUIPort.Edge == NodePortEdge.Left)
            {
                (_fromUIPort, _toUIPort) = (_toUIPort, _fromUIPort);
            }
        }

        private void AfterAdd()
        {
            {
                if (_fromUIPort.Port is OutputTrigger fromPort)
                {
                    _fromUIPort.ValidConnection(fromPort.Destination);
                }
            }
            {
                if (_toUIPort.Port is InputValue toPort)
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

            lineConnection.Init(this, Flow.GetConnection(_fromUIPort.Port, _toUIPort.Port));

            lineObject.transform.SetParent(_holder);
            rect.anchoredPosition = Vector2.zero;

            UILineRenderer lineRenderer = lineObject.AddComponent<UILineRenderer>();
            lineRenderer.Rect = rect;
            lineConnection.LineRenderer = lineRenderer;
            lineConnection.Source = _fromUIPort;
            lineConnection.Destination = _toUIPort;

            lineRenderer.Init(4);
            lineRenderer.Thickness = 6;
            lineRenderer.CornerRadius = 30;
            lineRenderer.CornerSegment = 5;

            // calculate bound;
            Vector3 headPosition;
            Vector3 tailPosition;
            headPosition = _fromUIPort.HandlePosition;
            tailPosition = _toUIPort.HandlePosition;

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

            _lines.Add(lineConnection);
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

        public void OnDrop(PointerEventData eventData)
        {
            // Debug.Log($"Drop {eventData.pointerDrag}");
            if (eventData.pointerDrag.TryGetComponent(out VariableBoardItem variableItem))
            {
                Vector3 mousePosition = Mouse.current.position.ReadValue();
                _openMenuPosition = mousePosition;

                Flow.AddGetVariableNode(variableItem.VariableName);
            }
        }
    }
}