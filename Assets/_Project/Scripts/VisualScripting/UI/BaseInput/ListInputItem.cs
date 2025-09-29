using UnityEngine;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class ListInputItem : MonoBehaviour
    {
        public ListInput ListInput { get; set; }
        public BaseInput Input { get; set; }

        public RectTransform Rect;
        public Button RemoveButton;

        private void Awake()
        {
            RemoveButton.onClick.AddListener(Remove);
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

        public object Get()
        {
            return Input.GetValue();
        }
    }
}