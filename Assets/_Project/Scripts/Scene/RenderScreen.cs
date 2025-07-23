using UnityEngine;
using UnityEngine.EventSystems;

public class RenderScreen : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Camera _camera;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            Vector2 normalizedPoint = Rect.PointToNormalized(_rectTransform.rect, localPoint);
            Vector3 worldPoint = _camera.ViewportToWorldPoint(normalizedPoint);

            EngineManager.Instance.Select(worldPoint);
        }
    }
}