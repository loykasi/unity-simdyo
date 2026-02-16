using UnityEngine;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class ListInputItem : MonoBehaviour
    {
        public ListInput ListInput { get; set; }
        public BaseInput Input { get; set; }

        public RectTransform Rect;
        [SerializeField] private Button _removeButton;
        [SerializeField] private float _removeButtonWidth;


        private void Awake()
        {
            _removeButton.onClick.AddListener(Remove);
        }

        public void Init(ListInput listInput, BaseInput input)
        {
            ListInput = listInput;
            Input = input;
            Input.OnSubmit += Edit;
            input.Rect.SetParent(Rect, false);
        }

        private void Edit(object value)
        {
            ListInput.Edit(this, value);
        }

        private void Remove()
        {
            ListInput.Remove(this);
        }

        public void Set(object value)
        {
            Input.SetValue(value);
        }

        public object Get()
        {
            return Input.GetValue();
        }

        public void SetWidth(float width)
        {
            Rect.sizeDelta = new Vector2(width, Rect.sizeDelta.y);
            Input.SetWidth(width - _removeButtonWidth);
        }
    }
}