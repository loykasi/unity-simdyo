using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class NodeBoard : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IScrollHandler, IDropHandler
    {
        public ScriptFlowGraph FlowGraph { get; set; }
        public ScriptFlow Flow => FlowGraph.Flow;
        public SceneEntity Entity => FlowGraph.Entity;

        [SerializeField] private RectTransform _rect;        
        [SerializeField] private Vector2 _zoomRange;
        [SerializeField] private RectTransform _holder;
        [SerializeField] private RectTransform _lineConnectionHolder;
        private Vector2 _offsetFromMouse;

        [Space]
        [SerializeField] private UINode _nodePrefab;

        [Space]
        [SerializeField] private NodeConnectionPreview _nodeConnectionPreview;
        [SerializeField] private float _sizePadding;

        [Space]
        [SerializeField] private NodeMenu _nodeMenu;

        // Dot grid background
        [Space]
        [SerializeField] private RawImage _backgroundImage;
        [SerializeField] private Material _backgroundMaterial;
        private readonly int _backgroundOffsetProperty = Shader.PropertyToID("_Offset");
        private readonly int _backgroundZoomProperty = Shader.PropertyToID("_Zoom");

        private bool _hasPort;

        private UINodePort _fromUIPort;
        private UINodePort _toUIPort;

        private Vector2 _previousMousePosition;

        private List<UINode> _nodes = new();
        private List<UILineConnection> _lines = new();

        private bool _isHover = false;
        private Vector3 _openMenuPosition;

        private bool _waitToAddNode = false;
        
        private readonly WaitForEndOfFrame _waitForEndOfFrame = new();

        // selection
        private List<RaycastResult> _results = new();
        private readonly PointerEventData _pointerData = new(EventSystem.current);
        private List<IGraphElement> _selectedElements = new();
        private bool _hasHandleSelection;
        private bool _isCursorMoving;

        // element dragging
        private Vector2 _initialMousePosition;

        private void Awake()
        {
            _backgroundImage.material = Instantiate(_backgroundMaterial);;
        }

        public void Init()
        {
            ClearBoard();

            Flow.OnNodeAdded += OnNodeAdded;
            Flow.OnNodeDeleted += OnNodeDeleted;
            Flow.OnConnectionDeleted += OnConnectionDeleted;

            LoadBoard();
        }

        public void Close()
        {
            if (Flow != null)
            {
                Flow.OnNodeAdded -= OnNodeAdded;
                Flow.OnNodeDeleted -= OnNodeDeleted;
                Flow.OnConnectionDeleted -= OnConnectionDeleted;
            }
            // _selectedElement = null;
            _selectedElements.Clear();
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
            _fromUIPort = null;
        }

        private void LoadBoard()
        {
            _holder.localPosition = Flow.Pan;
            _holder.localScale = Flow.Scale * Vector3.one;

            List<ScriptNode> nodes = Flow.Nodes;
            List<NodeConnection> connections = Flow.Connections;

            for (int i = 0; i < nodes.Count; i++)
            {
                AddNodeToBoard(nodes[i]);
            }

            RenderConnections(connections);

            // Debug.Log("======From System");
            // Debug.Log($"Node count: {nodes.Count}");
            // Debug.Log($"Connection count: {connections.Count}");

            // Debug.Log("======From UI");
            // Debug.Log($"Node count: {_nodes.Count}");
            // Debug.Log($"Connection count: {_lines.Count}");
        }

        private void AddNodeToBoard(ScriptNode scriptNode)
        {
            UINode node = Instantiate(_nodePrefab, _holder);

            node.Board = this;
            node.Node = scriptNode;

            _nodes.Add(node);
        }

        private void AddConnectionToBoard(UINodePort source, UINodePort destination)
        {
            int index = _lines.FindIndex(l => l.Source == source && l.Destination == destination);
            if (index != -1)
            {
                return;
            }

            UILineConnection lineConnection = CreateLine(source, destination);

            source.AddConnection(lineConnection);
            destination.AddConnection(lineConnection);

            _lines.Add(lineConnection);
        }

        private UILineConnection CreateLine(UINodePort source, UINodePort destination)
        {
            GameObject lineObject = new("line");

            lineObject.AddComponent<CanvasRenderer>();
            lineObject.transform.SetParent(_lineConnectionHolder, false);

            UILineConnection lineConnection = lineObject.AddComponent<UILineConnection>();
            RectTransform rect = lineObject.AddComponent<RectTransform>();
            UILineRenderer lineRenderer = lineObject.AddComponent<UILineRenderer>();

            lineConnection.Init(this, Flow.GetConnection(source.Port, destination.Port));

            lineRenderer.Rect = rect;
            lineConnection.LineRenderer = lineRenderer;
            lineConnection.Source = source;
            lineConnection.Destination = destination;

            lineRenderer.Init(4);
            lineRenderer.Thickness = 6;
            lineRenderer.CornerRadius = 30;
            lineRenderer.CornerSegment = 5;

            UpdateLineVisual(lineRenderer, source, destination);

            return lineConnection;
        }

        public void AddConnection(ScriptNode node)
        {
            StartCoroutine(AddConnectionNextFrame(node));
        }

        private IEnumerator AddConnectionNextFrame(ScriptNode node)
        {
            yield return _waitForEndOfFrame;

            IEnumerable<NodeConnection> connections = Flow.GetConnections(node);
            RenderConnections(connections);
        }

        private void RenderConnections(IEnumerable<NodeConnection> connections)
        {
            foreach (NodeConnection connection in connections)
            {
                UINode source = _nodes.Find((node) => node.Node == connection.Source.Node);
                UINode destination = _nodes.Find((node) => node.Node == connection.Destination.Node);
                UINodePort sourcePort = source.Ports.Find((port) => port.Port == connection.Source);
                UINodePort destinationPort = destination.Ports.Find((port) => port.Port == connection.Destination);

                if (connection.Source == null || connection.Destination == null || source == null || destination == null || sourcePort == null || destinationPort == null)
                {
                    continue;
                }

                AddConnectionToBoard(sourcePort, destinationPort);
            }
        }

        //==============================//==============================

        private void OnNodeAdded(ScriptNode scriptNode)
        {
            UINode node = Instantiate(_nodePrefab, _holder);
            _nodes.Add(node);

            node.Board = this;
            node.Node = scriptNode;
            // node.transform.position = _openMenuPosition;
            // scriptNode.Position = node.transform.localPosition;

            IEnumerable<NodeConnection> connections = Flow.GetConnections(scriptNode);

            int count = 0;
            foreach (NodeConnection connection in connections)
            {
                count++;
                UINode source = _nodes.Find((node) => node.Node == connection.Source.Node);
                UINode destination = _nodes.Find((node) => node.Node == connection.Destination.Node);
                UINodePort sourcePort = source.Ports.Find((port) => port.Port == connection.Source);
                UINodePort destinationPort = destination.Ports.Find((port) => port.Port == connection.Destination);

                if (connection.Source == null || connection.Destination == null || source == null || destination == null || sourcePort == null || destinationPort == null)
                {
                    continue;
                }

                _fromUIPort = sourcePort;
                _toUIPort = destinationPort;

                AddConnectionToBoard(sourcePort, destinationPort);
            }
        }

        public void AddNode(ScriptNode node)
        {
            if (_waitToAddNode)
            {
                _waitToAddNode = false;

                bool isSourcePort = _fromUIPort.Edge == NodePortEdge.Right;
                Flow.AddNode(node, ToBoardPosition(_openMenuPosition), _fromUIPort.Port, isSourcePort);

                return;
            }

            Flow.AddNode(node, ToBoardPosition(_openMenuPosition));
        }

        public void OnMenuClosed()
        {

            _fromUIPort = null;
            _waitToAddNode = false;
            _nodeConnectionPreview.EndPreviewConnect();
        }

        private void Update()
        {
            HandleSelection();
            HandleDelete();
            HandleMenu();
            HandleResetPan();
        }

        private void HandleSelection()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _isCursorMoving = false;
                _previousMousePosition = mousePosition;
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                if (_hasHandleSelection ||
                    mousePosition != _previousMousePosition)
                {
                    return;
                }

                SelectFromCursorPosition(mousePosition);
                return;
            }

            if (Mouse.current.leftButton.isPressed)
            {
                if (_hasHandleSelection)
                {
                    return;
                }

                if ((mousePosition - _previousMousePosition).sqrMagnitude == 0)
                {
                    return;
                }

                _isCursorMoving = true;
                _hasHandleSelection = true;

                SelectFromCursorPosition(mousePosition);
                if (_selectedElements.Count > 0)
                {
                    OnBeginDragElement();
                }
                return;
            }

            _hasHandleSelection = false;
        }

        private void SelectFromCursorPosition(Vector2 mousePosition)
        {
            _pointerData.position = mousePosition;
            EventSystem.current.RaycastAll(_pointerData, _results);

            bool isMultipleSelect = Keyboard.current.ctrlKey.isPressed;
            if (isMultipleSelect)
            {
                HandleMultiSelection();
            }
            else
            {
                HandleSingleSelection();
            }
        }

        private void HandleMultiSelection()
        {
            if (!TryGetTopGraphElement(out IGraphElement element))
            {
                return;
            }

            if (_selectedElements.Contains(element))
            {
                if (_isCursorMoving)
                {
                    return;
                }

                element.Unselect();
                _selectedElements.Remove(element);
                return;
            }
            element.Select();
            _selectedElements.Add(element);
        }

        private void HandleSingleSelection()
        {
            // results contains only node board
            if (_results.Count <= 1 && !_isCursorMoving)
            {
                DeselectAll();
                return;
            }

            if (!TryGetTopGraphElement(out IGraphElement element))
            {
                return;
            }

            if (_selectedElements.Contains(element))
            {
                return;
            }

            DeselectAll();
            element.Select();
            _selectedElements.Add(element);
        }

        private bool TryGetTopGraphElement(out IGraphElement element)
        {
            // Get graph element component from object or their parent
            GameObject targetGameObject = _results[0].gameObject;

            if (targetGameObject.TryGetComponent(out element))
            {
                return true;
            }

            element = targetGameObject.GetComponentInParent<IGraphElement>();
            return element != null;
        }

        private void DeselectAll()
        {
            foreach (var element in _selectedElements)
            {
                element.Unselect();
            }
            _selectedElements.Clear();
        }

        private void HandleResetPan()
        {
            if (Keyboard.current.tabKey.isPressed)
            {
                _holder.localScale = Vector3.one;
                _holder.localPosition = Vector3.zero;
            }
        }

        private void HandleZoom(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, eventData.position, eventData.enterEventCamera, out Vector2 mouseLocalPoint);

            float scale = _holder.localScale.x;
            scale += eventData.scrollDelta.y * scale / 1f * Time.unscaledDeltaTime;
            scale = Mathf.Clamp(scale, _zoomRange.x, _zoomRange.y);

            float delta = scale / _holder.localScale.x;
            _holder.localScale = scale * Vector3.one;
            _holder.anchoredPosition = mouseLocalPoint + (_holder.anchoredPosition - mouseLocalPoint) * delta;
            Flow.Scale = scale;

            UpdateBackgroundMaterial();
            _backgroundImage.materialForRendering.SetFloat(_backgroundZoomProperty, 1f / scale);
        }

        private void HandleMenu()
        {
            if (Mouse.current.rightButton.wasReleasedThisFrame && _isHover)
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
                // _selectedElement?.Delete();
                // _selectedElement = null;
                foreach (var element in _selectedElements)
                {
                    element.Delete();
                }
                _selectedElements.Clear();
            }
        }

        public void DeleteConnection(UILineConnection lineConnection)
        {
            Flow.Disconnect(lineConnection.Source.Port, lineConnection.Destination.Port);
            // DeleteConnectionVisual(lineConnection);
        }

        public void DeleteConnectionVisual(UILineConnection lineConnection)
        {
            lineConnection.DeleteVisual();
            _lines.Remove(lineConnection);
        }

        public void OnConnectionDeleted(NodeConnection connection)
        {
            UILineConnection connectionElement = _lines.Find(n => n.Connection == connection);
            if (connectionElement == null)
            {
                return;
            }

            DeleteConnectionVisual(connectionElement);
        }

        public void DeleteNode(UINode node)
        {
            Flow.DeleteNode(node.Node);
        }

        public void OnNodeDeleted(ScriptNode node)
        {
            int index = _nodes.FindIndex(n => n.Node == node);
            if (index == -1)
            {
                return;
            }

            UINode nodeElement = _nodes[index];
            
            for (int i = 0; i < nodeElement.Ports.Count; i++)
            {
                UINodePort port = nodeElement.Ports[i];
                for (int j = port.LineConnections.Count - 1; j >= 0; j--)
                {
                    DeleteConnectionVisual(port.LineConnections[j]);
                }
            }

            nodeElement.DeleteVisual();
            _nodes.Remove(nodeElement);
        }

        public void OnScroll(PointerEventData eventData)
        {
            HandleZoom(eventData);
        }

        public void OnBeginDragElement()
        {
            Debug.Log($"{Time.frameCount}::Begin drag element");
            foreach (var element in _selectedElements)
            {
                element.BeginMove();
            }
            _initialMousePosition = Mouse.current.position.ReadValue();
        }

        public void OnDragElement()
        {
            Debug.Log($"Dragging::{_selectedElements.Count}");
            Vector2 delta = Mouse.current.position.ReadValue() - _initialMousePosition;
            foreach (var element in _selectedElements)
            {
                element.Move(delta);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _offsetFromMouse = Mouse.current.position.ReadValue() - new Vector2(_holder.position.x, _holder.position.y);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _holder.position = Mouse.current.position.ReadValue() - _offsetFromMouse;
            Flow.Pan = _holder.localPosition;
            
            UpdateBackgroundMaterial();
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
            
            _nodeConnectionPreview.StartPreviewConnect(ToBoardPosition(startPosition), edge);
        }

        public void DragPreviewConnect(Vector3 mousePosition)
        {
            _nodeConnectionPreview.DragPreviewConnect(ToBoardPosition(mousePosition));
        }

        public void EndPreviewConnect()
        {
            TryConnect();
        }

        public void OnEnterPort(UINodePort toPort, Vector3 position)
        {
            IPort port = toPort.Port;
            if (_fromUIPort == null) return;
            if (!_fromUIPort.Port.CanConnect(port) || !port.CanConnect(_fromUIPort.Port)) return;

            _toUIPort = toPort;
            _hasPort = true;

            _nodeConnectionPreview.EnterPort(ToBoardPosition(position));
        }

        public void OnExitPort()
        {
            _hasPort = false;
            _nodeConnectionPreview.ExitPort();
        }

        private void TryConnect()
        {
            if (!_hasPort)
            {
                Vector3 mousePosition = Mouse.current.position.ReadValue();
                _openMenuPosition = mousePosition;

                _nodeMenu.Open(this, mousePosition, _fromUIPort.Port);
                _waitToAddNode = true;
                return;
            }

            SwapPort();

            _nodeConnectionPreview.EndPreviewConnect();

            UINode fromNode = _fromUIPort.UINode;
            IPort fromPort = _fromUIPort.Port;

            UINode toNode = _toUIPort.UINode;
            IPort toPort = _toUIPort.Port;

            if (Flow.TryConnect(_fromUIPort.Port, _toUIPort.Port))
            {
                _fromUIPort = fromNode.FindUIPort(fromPort);
                _toUIPort = toNode.FindUIPort(toPort);
                
                StartCoroutine(ConnectNextFrame(_fromUIPort, _toUIPort));
            }

            _fromUIPort = null;
            _toUIPort = null;
        }

        private IEnumerator ConnectNextFrame(UINodePort fromPort, UINodePort toPort)
        {
            yield return _waitForEndOfFrame;
            AddConnectionToBoard(fromPort, toPort);
        }

        private void SwapPort()
        {
            if (_fromUIPort.Edge == NodePortEdge.Left)
            {
                (_fromUIPort, _toUIPort) = (_toUIPort, _fromUIPort);
            }
        }

        public void UpdateLines(UILineConnection line)
        {
            UILineRenderer lineRenderer = line.LineRenderer;
            UpdateLineVisual(lineRenderer, line.Source, line.Destination);
        }

        public void UpdateLineVisual(UILineRenderer line, UINodePort fromPort, UINodePort toPort)
        {
            // temporary solution, should calculate manual instead
            // StartCoroutine(UpdateLineVisualNextFrame(line, fromPort, toPort));

            Vector3 fromPosition = ToBoardPosition(fromPort.HandlePosition);
            Vector3 toPosition = ToBoardPosition(toPort.HandlePosition);

            Vector3 size = fromPosition - toPosition;
            Vector3 center = (fromPosition + toPosition) / 2.0f;

            line.Points[0] = fromPosition - center;
            line.Points[1] = fromPosition + Vector3.right * 50f - center;
            line.Points[2] = toPosition + Vector3.left * 50f - center; ;
            line.Points[3] = toPosition - center; ;
            line.Rect.localPosition = center;
            line.Rect.sizeDelta = new Vector2(Mathf.Abs(size.x) + _sizePadding, Mathf.Abs(size.y) + _sizePadding);
            line.UpdateVertex();
        }

        // private IEnumerator UpdateLineVisualNextFrame(UILineRenderer line, UINodePort fromPort, UINodePort toPort)
        // {
        //     yield return _waitForEndOfFrame;

        //     Vector3 fromPosition = ToBoardPosition(fromPort.HandlePosition);
        //     Vector3 toPosition = ToBoardPosition(toPort.HandlePosition);

        //     Vector3 size = fromPosition - toPosition;
        //     Vector3 center = (fromPosition + toPosition) / 2.0f;

        //     line.Points[0] = fromPosition - center;
        //     line.Points[1] = fromPosition + Vector3.right * 50f - center;
        //     line.Points[2] = toPosition + Vector3.left * 50f - center; ;
        //     line.Points[3] = toPosition - center; ;
        //     line.Rect.localPosition = center;
        //     line.Rect.sizeDelta = new Vector2(Mathf.Abs(size.x) + _sizePadding, Mathf.Abs(size.y) + _sizePadding);
        //     line.UpdateVertex();
        // }

        // handle drag and drop to create node
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.TryGetComponent(out VariableBoardItem variableItem))
            {
                Vector3 position = GetMouseBoardPosition();
                Flow.AddGetVariableNode(variableItem.Variable, position);
            }

            if (eventData.pointerDrag.TryGetComponent(out FunctionBoardItem functionItem))
            {
                Vector3 position = GetMouseBoardPosition();
                Flow.AddFunctionCallNode(functionItem.Function, position);
            }

            if (eventData.pointerDrag.TryGetComponent(out NodeListItem nodeListItem))
            {
                Flow.AddNode(nodeListItem.NodeData, GetMouseBoardPosition());
            }
        }

        private Vector3 GetMouseBoardPosition()
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            return ToBoardPosition(mousePosition);
        }

        private Vector3 ToBoardPosition(Vector3 worldPosition)
        {
            // world to screen point
            // screen point to canvas
            Camera camera = EngineManager.Instance.EditorCamera;
            Vector2 screenPosition = camera.WorldToScreenPoint(worldPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_holder, screenPosition, camera, out Vector2 point);
            return point;
            // return (worldPosition - _holder.position) * 1f / _holder.localScale.x;
        }

        private void UpdateBackgroundMaterial()
        {
            Vector2 tiling = new(_rect.rect.size.x / _rect.rect.size.y, 1f);
            Vector2 offset = - _holder.localPosition;
            _backgroundImage.materialForRendering.SetVector(_backgroundOffsetProperty, offset);
        }
    }
}