using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class FunctionBoardItem : MonoBehaviour, IDragHandler
    {
        public FunctionBoard Board { get; set; }
        public ScriptFunction Function;

        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Button _editButton;

        private void Awake()
        {
            _editButton.onClick.AddListener(Edit);
        }

        private void OnDisable()
        {
            Function.OnUpdated -= OnUpdated;
        }

        private void OnUpdated()
        {
            _label.text = Function.Name;
        }

        private void Edit()
        {
            Board.Select(this);
        }

        public void Init(FunctionBoard board, ScriptFunction function)
        {
            Board = board;
            Function = function;
            
            Function.OnUpdated += OnUpdated;

            _label.text = function.Name;
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }
    }
}