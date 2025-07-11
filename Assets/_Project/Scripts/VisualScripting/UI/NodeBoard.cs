using System;
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
    [SerializeField] private UILineRenderer _previewConnectLine;
    private bool _hasPort;
    private Vector3 _portPosition;

    private UINode _fromUINode;
    private ScriptNode _fromNode;
    private IPort _fromPort;

    private UINode _toUINode;
    private ScriptNode _toNode;
    private IPort _toPort;


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

    private void LoadBoard()
    {
        if (TargetVisualScripting.startNode != null)
        {
            UINode node = Instantiate(_nodePrefab, _holder);
            node.Node = TargetVisualScripting.startNode;
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

    public void StartPreviewConnect(UINode node, ScriptNode fromNode, IPort port, Vector3 startPosition)
    {
        _fromUINode = node;
        _fromNode = fromNode;
        _fromPort = port;
        _previewConnectLine.gameObject.SetActive(true);
        _previewConnectLine.Points[0] = startPosition;
    }

    public void DragPreviewConnect(Vector3 mousePosition)
    {
        if (_hasPort) return;

        _previewConnectLine.Points[1] = mousePosition;
        _previewConnectLine.UpdateVertex();
    }

    public void EndPreviewConnect()
    {
        _previewConnectLine.gameObject.SetActive(false);

        TryConnect();
    }

    public void OnEnterPort(UINode node, ScriptNode toNode, IPort port, Vector3 position)
    {
        if (_fromPort == null) return;
        if (!_fromPort.CanConnectTo(port)) return;

        _toUINode = node;
        _toNode = toNode;
        _toPort = port;
        _hasPort = true;
        _portPosition = position;

        _previewConnectLine.Points[1] = _portPosition;
        _previewConnectLine.UpdateVertex();
    }

    public void OnExitPort()
    {
        _hasPort = false;
    }

    private void TryConnect()
    {
        if (!_hasPort) return;

        if (TargetVisualScripting.TryConnect(_fromNode, _fromPort, _toNode, _toPort))
        {
            AddConnectionLine();
        }
        
    }

    private void AddConnectionLine()
    {
        GameObject lineObject = new("line");

        RectTransform rect = lineObject.AddComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        lineObject.AddComponent<CanvasRenderer>();

        UILineRenderer lineRenderer = lineObject.AddComponent<UILineRenderer>();
        lineRenderer.Init(2);
        lineRenderer.Thickness = 10;

        // Vector3 startPos = ScreenToCenter.GetPostionFromCenter(_previewConnectLine.Points[0]);
        // Vector3 endPos = ScreenToCenter.GetPostionFromCenter(_previewConnectLine.Points[1]);
        // lineRenderer.Points[0] = startPos;
        // lineRenderer.Points[1] = endPos;

        Vector3 startPos = _previewConnectLine.Points[0];
        Vector3 endPos = _previewConnectLine.Points[1];
        lineRenderer.Points[0] = startPos;
        lineRenderer.Points[1] = endPos;
        lineObject.transform.SetParent(_holder);

        _fromUINode.AddConnectionLine(lineRenderer, startPos, true);
        _toUINode.AddConnectionLine(lineRenderer, endPos, false);
    }
}