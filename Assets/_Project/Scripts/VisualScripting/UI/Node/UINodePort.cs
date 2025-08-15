using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum NodePortEdge
{
    Left,
    Right
}

public abstract class UINodePort : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public IPort Port;
    public UINode UINode { get; set; }
    public abstract NodePortEdge Edge { get; }
    public Vector3 HandlePosition => _portHandle.transform.position;

    public RectTransform Rect;
    [SerializeField] private RectTransform _portHandle;
    [SerializeField] protected TextMeshProUGUI _label;
    private NodeBoard _nodeBoard;

    protected List<UILineConnection> _lineConnections = new();

    private void Awake()
    {
        _nodeBoard = NodeBoard.Instance;
    }

    public virtual void Init()
    {
        if (_label != null)
        {
            if (Port.ShouldShowLabel)
            {
                _label.text = Port.Key;
            }
            else
            {
                _label.gameObject.SetActive(false);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _nodeBoard.StartPreviewConnect(this, _portHandle.position, Edge);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _nodeBoard.DragPreviewConnect(Mouse.current.position.ReadValue());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _nodeBoard.EndPreviewConnect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _nodeBoard.OnEnterPort(this, _portHandle.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _nodeBoard.OnExitPort();
    }

    public void AddConnection(UILineConnection lineConnection)
    {
        _lineConnections.Add(lineConnection);
    }

    public void UpdateLines()
    {
        for (int i = 0; i < _lineConnections.Count; i++)
        {
            _nodeBoard.UpdateLines(_lineConnections[i].LineRenderer, Edge, _portHandle.position);
        }
    }

    public void DeleteConnection(UILineConnection lineConnection, IPort other)
    {
        _lineConnections.Remove(lineConnection);
        Port.Disconnect(other);
    }

    public void DeleteAllLines()
    {
        for (int i = 0; i < _lineConnections.Count; i++)
        {
            _lineConnections[i].Delete();
        }
    }

    public virtual void ValidConnection(IPort port)
    {
        for (int i = 0; i < _lineConnections.Count; i++)
        {
            if (_lineConnections[i].Destination.Port != port)
            {
                _lineConnections[i].Delete();
            }
        }
    }
}