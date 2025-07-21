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

public class UINodePort : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public IPort Port;
    public UINode UINode;
    public NodePortEdge Edge => _edge;

    [SerializeField] private NodePortEdge _edge;
    [SerializeField] private RectTransform _portHandle;
    [SerializeField] private TMP_InputField _inputField;
    private NodeBoard _nodeBoard;

    protected List<UILineConnection> _lineConnections = new();

    private void Awake()
    {
        _nodeBoard = NodeBoard.Instance;
    }

    public void Init()
    {
        if (Port is ValueOutput valueOutput && _inputField != null)
        {
            if (valueOutput.IsUseInputField)
            {
                _inputField.gameObject.SetActive(true);
            }
            else
            {
                _inputField.gameObject.SetActive(false);
            }
        }

        if (Port is ValueInput valueInput && _inputField != null)
        {
            if (valueInput.UseOptionalInput)
            {
                _inputField.gameObject.SetActive(true);
            }
            else
            {
                _inputField.gameObject.SetActive(false);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _nodeBoard.StartPreviewConnect(this, _portHandle.position, _edge);
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

    public void OnEndEdit(string value)
    {
        if (Port is ValueOutput valueOutput)
        {
            valueOutput.SetValue(value);
        }

        if (Port is ValueInput valueInput)
        {
            valueInput.SetValue(value);
        }
    }

    public void AddConnection(UILineConnection lineConnection)
    {
        _lineConnections.Add(lineConnection);
    }

    public void UpdateLines()
    {
        for (int i = 0; i < _lineConnections.Count; i++)
        {
            _nodeBoard.UpdateLines(_lineConnections[i].LineRenderer, _edge, _portHandle.position);
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