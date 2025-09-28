using UnityEngine;
using UnityEngine.EventSystems;

public class GameCanvasInteraction : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private RectTransform _rect;

    public void OnPointerDown(PointerEventData eventData)
    {
        Camera camera = EngineManager.Instance.SceneCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        Vector2 normalizedPoint = Rect.PointToNormalized(_rect.rect, localPoint);

        Vector3 screenPoint = camera.ViewportToScreenPoint(normalizedPoint);

        ObjectManager.Instance.Click(screenPoint);
    }
}