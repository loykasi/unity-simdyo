using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UINode : MonoBehaviour, IDragHandler, IBeginDragHandler, IGraphElement, IPointerEnterHandler, IPointerExitHandler
{
    public ScriptNode Node
    {
        get => _node;
        set
        {
            _node = value;
            UpdateNodeUI();
        }
    }

    private ScriptNode _node;

    public List<UINodePort> Ports = new();

    [SerializeField] private TMP_Text _nodeTitle;
    [SerializeField] private GameObject _selectedBorder;

    [SerializeField] private RectTransform _inputHolder;
    [SerializeField] private RectTransform _outputHolder;

    [SerializeField] private UINodePort _inputTriggerPrefab;
    [SerializeField] private UINodePort _inputValuePrefab;
    [SerializeField] private UINodePort _outputTriggerPrefab;
    [SerializeField] private UINodePort _outputValuePrefab;

    private Vector2 _offsetFromMouse;
    private bool _isMouseOver = false;

    private void UpdateNodeUI()
    {
        if (Node == null) return;

        transform.localPosition = Node.Positon;
        _nodeTitle.SetText(Node.Title);

        for (int i = 0; i < Node.InputTriggers.Count; i++)
        {
            UINodePort port = Instantiate(_inputTriggerPrefab, _inputHolder);
            port.UINode = this;
            port.Port = Node.InputTriggers[i];
            port.Init();
            Ports.Add(port);
        }

        for (int i = 0; i < Node.OutputTriggers.Count; i++)
        {
            UINodePort port = Instantiate(_outputTriggerPrefab, _outputHolder);
            port.UINode = this;
            port.Port = Node.OutputTriggers[i];
            port.Init();
            Ports.Add(port);
        }

        for (int i = 0; i < Node.ValueInputs.Count; i++)
        {
            UINodePort port = Instantiate(_inputValuePrefab, _inputHolder);
            port.UINode = this;
            port.Port = Node.ValueInputs[i];
            port.Init();
            Ports.Add(port);
        }

        for (int i = 0; i < Node.ValueOutputs.Count; i++)
        {
            UINodePort port = Instantiate(_outputValuePrefab, _outputHolder);
            port.UINode = this;
            port.Port = Node.ValueOutputs[i];
            port.Init();
            Ports.Add(port);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(_inputHolder);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_outputHolder);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _offsetFromMouse = Mouse.current.position.ReadValue() - new Vector2(transform.position.x, transform.position.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Mouse.current.position.ReadValue() - _offsetFromMouse;
        Node.Positon = transform.localPosition;

        for (int i = 0; i < Ports.Count; i++)
        {
            Ports[i].UpdateLines();
        }
    }

    public void Select()
    {
        _selectedBorder.SetActive(true);
    }

    public void Delete()
    {
        for (int i = 0; i < Ports.Count; i++)
        {
            Ports[i].DeleteAllLines();
        }
        Destroy(gameObject);
    }

    public void Unselect()
    {
        _selectedBorder.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMouseOver = false;
    }
}
