using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum NodePortType
{
    InputTrigger,
    OutputTrigger,
    ValueInput,
    ValueOutput,
}

public class UINodePort : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public IPort Port;
    public UINode UINode;

    [SerializeField] private RectTransform _portHandle;
    [SerializeField] private TMP_InputField _inputField;
    private NodeBoard _nodeBoard;

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
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _nodeBoard.StartPreviewConnect(UINode, UINode.Node, Port, _portHandle.position);
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
        _nodeBoard.OnEnterPort(UINode, UINode.Node, Port, _portHandle.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _nodeBoard.OnExitPort();
    }

    public void OnEndEdit(string value)
    {
        Debug.Log("save");
        ((ValueOutput)Port).SetValue(value);
    }
}