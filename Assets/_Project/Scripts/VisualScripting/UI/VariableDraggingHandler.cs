using UnityEngine;
using UnityEngine.EventSystems;

namespace Loykas.Scripting
{
    public class VariableDraggingHandler : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            GameObject target = eventData.pointerDrag;
            if (target.TryGetComponent(out VariableBoardItem item))
            {
                Debug.Log("Add get variable");
            }
        }
    }
}