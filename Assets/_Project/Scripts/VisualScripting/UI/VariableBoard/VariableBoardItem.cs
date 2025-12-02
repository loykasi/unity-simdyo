using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class VariableBoardItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Variable Variable;
        private VariableBoard _variableBoard;

        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Button _editButton;

        private void Awake()
        {
            _editButton.onClick.AddListener(Edit);
        }

        private void OnDisable()
        {
            Variable.OnUpdated -= OnVariableUpdated;
        }

        public void Init(VariableBoard variableBoard, Variable variable)
        {
            _variableBoard = variableBoard;
            Variable = variable;
            Variable.OnUpdated += OnVariableUpdated;

            _label.text = variable.Name;
        }

        private void OnVariableUpdated()
        {
            _label.text = Variable.Name;
        }

        private void Edit()
        {
            _variableBoard.Select(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragPreviewSystem.Instance.BeginDrag(transform.position, Variable.Name);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragPreviewSystem.Instance.EndDrag();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            CursorSystem.Instance.SetCursor(CursorType.Grab);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CursorSystem.Instance.ToDefault();
        }
    }
}