using UnityEngine;
using UnityEngine.EventSystems;

public class UILineConnection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IGraphElement
{
    public NodeBoard Board { get; set; }
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
        Board.DeleteConnection(this);
    }

    public void DeleteVisual()
    {
        Source.DeleteConnection(this);
        Destination.DeleteConnection(this);
        Destroy(gameObject);
        Debug.Log("Delete line");
    }

    public void Unselect()
    {
        LineRenderer.color = Color.white;
        LineRenderer.UpdateVertex();
    }
}