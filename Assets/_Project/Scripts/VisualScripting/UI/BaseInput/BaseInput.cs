using UnityEngine;
using UnityEngine.Events;

namespace Loykas.Scripting
{
    public abstract class BaseInput : MonoBehaviour
    {
        public DataType Type { get; }
        public Variable ValueInstance { get; set; }

        public UnityAction<object> OnSubmit;
        public UnityAction OnValueUpdated;

        public RectTransform Rect;
        public Vector2 Size
        {
            get => Rect.sizeDelta;
            set => Rect.sizeDelta = value;
        }

        public virtual void Enable()
        {
            gameObject.SetActive(true);
        }
        public virtual void Disable()
        {
            gameObject.SetActive(false);
        }

        public abstract object GetValue();
        public virtual void SetValue(object value) { }
        public virtual void SetValueInstance(Variable value)
        {
            ValueInstance = value;
        }

        public virtual void SetWidth(float width)
        {
            
        }
    }
}