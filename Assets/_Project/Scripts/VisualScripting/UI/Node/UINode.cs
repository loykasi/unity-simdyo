using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class UINode : MonoBehaviour, IDragHandler, IBeginDragHandler, IGraphElement, IPointerEnterHandler, IPointerExitHandler
    {
        public NodeBoard Board { get; set; }
        public ScriptNode Node
        {
            get => _node;
            set
            {
                SetNode(value);
            }
        }

        private ScriptNode _node;

        public UINodePort EnterPort;
        [HideInInspector] public List<UINodePort> Ports = new();
        [HideInInspector] public List<UINodePort> InputPorts = new();
        [HideInInspector] public List<UINodePort> OutputTriggerPorts = new();
        [HideInInspector] public List<UINodePort> OutputValuePorts = new();

        [Header("References")]
        [SerializeField] private TMP_Text _nodeTitle;
        [SerializeField] private RectTransform _selectedBorder;
        [SerializeField] private float _borderSize;

        [Header("Node Holders")]
        [SerializeField] private RectTransform _inputHolder;
        [SerializeField] private RectTransform _outputHolder;

        [Header("Head and body")]
        [SerializeField] private RectTransform _head;
        [SerializeField] private RectTransform _body;

        [Header("Prefabs")]
        [SerializeField] private UINodePort _inputTriggerPrefab;
        [SerializeField] private UINodePort _inputValuePrefab;
        [SerializeField] private UINodePort _outputTriggerPrefab;
        [SerializeField] private UINodePort _outputValuePrefab;

        private Vector2 _offsetFromMouse;
        // private bool _isMouseOver = false;

        private readonly float _inputOutputDistance = 10f;
        private float _minWidth = 50f;
        private readonly float _topBottomPadding = 20f;


        private void SetNode(ScriptNode node)
        {
            if (_node != null)
            {
                _node.OnNodeUpdated -= UpdateUI;
            }

            _node = node;
            _node.OnNodeUpdated += UpdateUI;
            transform.localPosition = _node.Position;

            Init();
        }

        void OnDisable()
        {
            if (_node != null)
            {
                _node.OnNodeUpdated -= UpdateUI;
            }
        }

        private void Init()
        {
            if (Node == null) return;
            
            transform.localPosition = Node.Position;

            // title
            string title = Node.GetNameKey();
            if (Node.ShouldLocalized)
            {
                // Debug.Log(title);
                title = GlobalLocalization.Instance.GetValue(title);   
            }
            _nodeTitle.SetText(title);
            
            Vector2 labelSize = _nodeTitle.GetPreferredValues();
            _minWidth = labelSize.x + 20f;

            if (Node.HasInputTrigger)
            {
                UINodePort port = Instantiate(_inputTriggerPrefab, _inputHolder);
                port.UINode = this;
                port.Port = Node.EnterTrigger;
                port.Init();
                Ports.Add(port);
                EnterPort = port;
            }

            for (int i = 0; i < Node.OutputTriggers.Count; i++)
            {
                UINodePort port = Instantiate(_outputTriggerPrefab, _outputHolder);
                port.UINode = this;
                port.Port = Node.OutputTriggers[i];
                port.Init();
                Ports.Add(port);
                OutputTriggerPorts.Add(port);
            }

            for (int i = 0; i < Node.ValueInputs.Count; i++)
            {
                UINodePort port = Instantiate(_inputValuePrefab, _inputHolder);
                port.UINode = this;
                port.Port = Node.ValueInputs[i];
                port.Init();
                Ports.Add(port);
                InputPorts.Add(port);
            }

            for (int i = 0; i < Node.ValueOutputs.Count; i++)
            {
                UINodePort port = Instantiate(_outputValuePrefab, _outputHolder);
                port.UINode = this;
                port.Port = Node.ValueOutputs[i];
                port.Init();
                Ports.Add(port);
                OutputValuePorts.Add(port);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_inputHolder);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_outputHolder);

            UpdateSize();
        }

        private void UpdateUI()
        {
            // for (int i = 0; i < Ports.Count; i++)
            // {
            //     Ports[i].UpdateUI();
            // }

            for (int i = 0; i < Ports.Count; i++)
            {
                Ports[i].DeleteAllLines();
                Destroy(Ports[i].gameObject);
            }
            Ports.Clear();
            InputPorts.Clear();
            OutputTriggerPorts.Clear();
            OutputValuePorts.Clear();

            Init();
        }

        public void UpdateSize()
        {
            Vector2 inputSize = GetPortGroupMaxSize(InputPorts);
            Vector2 outputValueSize = GetPortGroupMaxSize(OutputValuePorts);
            Vector2 outputTriggerSize = GetPortGroupMaxSize(OutputTriggerPorts);

            // float inputHeight = _inputHolder.sizeDelta.y;
            // float outputHeight = _outputHolder.sizeDelta.y;
            // float bodyHeight = (inputHeight > outputHeight ? inputHeight : outputHeight) + _topBottomPadding;

            float inputHeight = inputSize.y + (Node.HasInputTrigger ? EnterPort.Rect.sizeDelta.y : 0) + (InputPorts.Count + (Node.HasInputTrigger ? 1 : 0) - 1) * 10f;
            float outputHeight = outputValueSize.y + outputTriggerSize.y + (OutputValuePorts.Count + OutputTriggerPorts.Count - 1) * 10f;
            float bodyHeight = (inputHeight > outputHeight ? inputHeight : outputHeight) + _topBottomPadding;

            float x = inputSize.x + Mathf.Max(outputValueSize.x, outputTriggerSize.x) + _inputOutputDistance;
            x = Mathf.Max(x, _minWidth);

            _head.sizeDelta = new Vector2(x, _head.sizeDelta.y);
            _body.sizeDelta = new Vector2(x, bodyHeight);

            UpdateBorder();
            UpdateLineVisual();
        }

        private void UpdateBorder()
        {
            Vector2 nodeSize = new
            (
                _head.sizeDelta.x,
                _head.sizeDelta.y + _body.sizeDelta.y
            );

            Vector2 borderSize = nodeSize + _borderSize * 2 * Vector2.one;
            _selectedBorder.sizeDelta = borderSize;
        }

        private Vector2 GetPortGroupMaxSize(List<UINodePort> ports)
        {
            if (ports.Count == 0)
            {
                return Vector2.zero;
            }

            float y = 0;
            float x = ports[0].Rect.sizeDelta.x;
            for (int i = 0; i < ports.Count; i++)
            {
                Vector2 size = ports[i].Rect.sizeDelta;
                if (i != 0)
                {
                    if (size.x > x)
                    {
                        x = size.x;
                    }
                }

                y += size.y;
            }

            return new Vector2(x, y);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _offsetFromMouse = Mouse.current.position.ReadValue() - new Vector2(transform.position.x, transform.position.y);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Mouse.current.position.ReadValue() - _offsetFromMouse;
            Node.Position = transform.localPosition;

            UpdateLineVisual();
        }

        public void UpdateLineVisual()
        {
            for (int i = 0; i < Ports.Count; i++)
            {
                Ports[i].UpdateLines();
            }
        }

        public void Select()
        {
            _selectedBorder.gameObject.SetActive(true);
        }

        public void Delete()
        {
            Board.DeleteNode(this);
        }

        public void DeleteVisual()
        {
            Destroy(gameObject);
        }

        public void Unselect()
        {
            _selectedBorder.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // _isMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // _isMouseOver = false;
        }

        public UINodePort FindUIPort(IPort port)
        {
            return Ports.Find(p => p.Port == port);
        }
    }
}