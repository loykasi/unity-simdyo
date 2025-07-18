using UnityEngine;
using UnityEngine.EventSystems;

public class UILineConnection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IGraphElement
{
    public UILineRenderer LineRenderer;
    public UINodePort Source;
    public UINodePort Destination;

    public void OnPointerEnter(PointerEventData eventData)
    {
        LineRenderer.Thickness = 10;
        LineRenderer.UpdateVertex();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LineRenderer.Thickness = 3;
        LineRenderer.UpdateVertex();
    }

    public void Select()
    {
        LineRenderer.color = Color.blue;
        LineRenderer.UpdateVertex();
    }
    
    public void Delete()
    {
        Source.DeleteConnection(this, Destination.Port);
        Destination.DeleteConnection(this, Source.Port);
        Destroy(gameObject);
    }
}