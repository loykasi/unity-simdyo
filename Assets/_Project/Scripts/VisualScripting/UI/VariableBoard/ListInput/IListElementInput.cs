using UnityEngine;
using UnityEngine.Events;

public class ListElementInput : MonoBehaviour
{
    public UnityAction<ListElementInput, object> OnEndEdit { get; set; }
    public UnityAction<ListElementInput> OnRemove { get; set; }

    public RectTransform Rect;
    public virtual object DefaultValue { get; }

    public virtual void SetValue(object value)
    {

    }
}