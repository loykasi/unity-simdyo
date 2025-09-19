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

    private NodeBoard NodeBoard => UINode.Board;

    public RectTransform Rect;
    [SerializeField] private RectTransform _portHandle;
    [SerializeField] protected TextMeshProUGUI _label;

    protected List<UILineConnection> _lineConnections = new();

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
        Debug.Log($"{_label.text}: {_label.rectTransform.sizeDelta}");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        NodeBoard.StartPreviewConnect(this, _portHandle.position, Edge);
    }

    public void OnDrag(PointerEventData eventData)
    {
        NodeBoard.DragPreviewConnect(Mouse.current.position.ReadValue());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        NodeBoard.EndPreviewConnect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        NodeBoard.OnEnterPort(this, _portHandle.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        NodeBoard.OnExitPort();
    }

    public void AddConnection(UILineConnection lineConnection)
    {
        _lineConnections.Add(lineConnection);
    }

    public void UpdateLines()
    {
        for (int i = 0; i < _lineConnections.Count; i++)
        {
            NodeBoard.UpdateLines(_lineConnections[i].LineRenderer, Edge, _portHandle.position);
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

    public virtual void AfterAdd()
    {
        
    }
}