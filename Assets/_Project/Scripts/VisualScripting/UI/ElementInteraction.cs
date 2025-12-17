using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ElementInteraction : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}