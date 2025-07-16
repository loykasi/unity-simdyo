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
    private bool _hasPort;

    private UINodePort _fromUIPort;
    private ScriptNode _fromNode;
    private IPort _fromPort;

    private UINodePort _toUIPort;
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
        }

    }

    private void AddConnectionLine()
    {
        GameObject lineObject = new("line");

        RectTransform rect = lineObject.AddComponent<RectTransform>();
        lineObject.transform.SetParent(_holder);
        rect.anchoredPosition = Vector2.zero;
        lineObject.AddComponent<CanvasRenderer>();

        UILineRenderer lineRenderer = lineObject.AddComponent<UILineRenderer>();
        lineRenderer.Init(4);
        lineRenderer.Thickness = 5;
        lineRenderer.CornerRadius = 30;
        lineRenderer.CornerSegment = 5;

        // Vector3 startPos = ScreenToCenter.GetPostionFromCenter(_previewConnectLine.Points[0]);
        // Vector3 endPos = ScreenToCenter.GetPostionFromCenter(_previewConnectLine.Points[1]);
        // lineRenderer.Points[0] = startPos;
        // lineRenderer.Points[1] = endPos;

        UILineRenderer previewLineRender = _nodeConnectionPreview.LineRenderer;
        lineRenderer.Points[0] = previewLineRender.Points[0] - _holder.position;
        lineRenderer.Points[1] = previewLineRender.Points[1] - _holder.position;
        lineRenderer.Points[2] = previewLineRender.Points[2] - _holder.position;
        lineRenderer.Points[3] = previewLineRender.Points[3] - _holder.position;

        // _fromUINode.AddConnectionLine(lineRenderer, true);
        // _toUINode.AddConnectionLine(lineRenderer, false);
        _fromUIPort.AddConnection(lineRenderer);
        _toUIPort.AddConnection(lineRenderer);
    }

    public void UpdateLines(List<UILineRenderer> lines, NodePortEdge edge, Vector3 portPosition)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            Vector3 position = portPosition - _holder.position;

            switch (edge)
            {
                case NodePortEdge.Left:
                    lines[i].Points[2] = position + Vector3.left * 50f;
                    lines[i].Points[3] = position;
                    lines[i].UpdateVertex();
                    break;
                case NodePortEdge.Right:
                    lines[i].Points[0] = position;
                    lines[i].Points[1] = position + Vector3.right * 50f;
                    lines[i].UpdateVertex();
                    break;
            }
        }
    }
}