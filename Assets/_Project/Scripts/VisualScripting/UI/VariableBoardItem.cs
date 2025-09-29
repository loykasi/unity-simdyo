using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class VariableBoardItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
    {
        public string VariableName => _nameInputField.text;

        [SerializeField] private RectTransform _rect;
        [SerializeField] private TMP_InputField _nameInputField;
        // [SerializeField] private TMP_Dropdown _typeDropdown;
        [SerializeField] private TypeInput _typeInput;
        [SerializeField] private Button _removeButton;

        [Header("Input")]
        [SerializeField] private RectTransform _inputHolder;
        [SerializeField] private UIInputData _inputDataReference;
        private BaseInput _input;

        private Variable _variable;
        private VariableBoard _variableBoard;

        private readonly float _width = 300f;
        private readonly float _verticalPadding = 10f;
        private readonly float _titleAndTypeHeight = 60f;

        private void Start()
        {
            InitDropDown();

            _removeButton.onClick.AddListener(OnRemove);
        }

        private void InitDropDown()
        {
            _typeInput.OnSubmit += OnTypeChanged;
        }

        public void Init(string name, VariableBoard variableBoard)
        {
            _nameInputField.text = name;

            _variableBoard = variableBoard;
            _variable = _variableBoard.FlowGraph.Flow.GetVariable(name);

            OnTypeChanged(_typeInput.GetValue());
        }

        // NEED TO FIX THIS
        public void Init(string name, ScriptDataType type, object value, VariableBoard variableBoard)
        {
            InitDropDown();

            _nameInputField.text = name;
            _typeInput.SetValue(type);

            _variableBoard = variableBoard;
            _variable = _variableBoard.FlowGraph.Flow.GetVariable(name);

            ChangeInput(type);
        }

        private void OnTypeChanged(object value)
        {
            ScriptDataType type = (ScriptDataType)value;

            ValueHandler.SetDefaultValue(_variable, type);

            ChangeInput(type);
        }

        private void ChangeInput(ScriptDataType type)
        {
            if (_input != null)
            {
                Destroy(_input.gameObject);
            }

            _input = _inputDataReference.Get(type);
            _input.Rect.SetParent(_inputHolder, false);
            _input.Enable();

            _input.OnSubmit += OnInputSubmit;
            _input.SetValueInstance(_variable);
        }

        private void OnInputSubmit(object value)
        {
            UpdateSize();
        }

        private void OnRemove()
        {
            _variableBoard.RemoveVariable(_nameInputField.text, this);
        }

        private void UpdateSize()
        {
            _rect.sizeDelta = new Vector2
            (
                _width,
                _verticalPadding + _titleAndTypeHeight + _input.Size.y
            );
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Debug.Log("drag variable");
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }

        public void OnDrag(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }
    }
}