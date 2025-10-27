using UnityEngine;
using UnityEngine.UI;

public class RotateController : Singleton<RotateController>
{
    [SerializeField] private float _snapRadius;
    [SerializeField] private RectTransform _visualization;
    
    [SerializeField] private RectTransform _mouseRotateRect;
    [SerializeField] private Image _mouseRotateImage;

    private Vector3 _startMouseDirection;

    public float GetSnapRadiusWorld()
    {
        Camera camera = EngineManager.Instance.EditorCamera;
        float scale = camera.orthographicSize * 2 / Screen.height * EngineManager.Instance.CanvasScale;
        return _snapRadius * scale;
    }

    public bool IsSnapping(Vector3 mousePosition, Vector3 entityPosition)
    {
        float centerToMouseSqrDist = (mousePosition - entityPosition).sqrMagnitude;
        float radius = GetSnapRadiusWorld();
        return centerToMouseSqrDist < radius * radius;
    }

    public void EnableVisualization(Vector3 entityPosition, Vector3 mousePosition)
    {
        _startMouseDirection = mousePosition - entityPosition;
        float angle = Mathf.Atan2(_startMouseDirection.y, _startMouseDirection.x) * Mathf.Rad2Deg + 90f;
        _mouseRotateRect.rotation = Quaternion.Euler(0f, 0f, angle);
        Debug.Log(angle);

        _visualization.gameObject.SetActive(true);
    }

    public void DisableVisualization()
    {
        _visualization.gameObject.SetActive(false);
    }

    public void UpdateUI(Vector3 worldPosition, Vector3 mousePosition)
    {
        UpdateMouseRotateIndicator(worldPosition, mousePosition);

        Camera camera = EngineManager.Instance.EditorCamera;
        Vector3 screenPoint = camera.WorldToScreenPoint(worldPosition);

        _visualization.position = screenPoint;
        _visualization.sizeDelta = _snapRadius * 2f * Vector2.one;
    }
    
    private void UpdateMouseRotateIndicator(Vector3 worldPosition, Vector3 mousePosition)
    {
        Vector3 toDirection = mousePosition - worldPosition;
        float angle = Vector3.SignedAngle(_startMouseDirection, toDirection, Vector3.forward);

        if (IsSnapping(mousePosition, worldPosition))
        {
            angle = Mathf.Round(angle / 15f) * 15f;
        }

        _mouseRotateImage.fillClockwise = angle < 0;
        _mouseRotateImage.fillAmount = Mathf.Abs(angle / 360f);

        Camera camera = EngineManager.Instance.EditorCamera;
        Vector3 screenPoint = camera.WorldToScreenPoint(worldPosition);
        Vector3 mouseScreenPoint = camera.WorldToScreenPoint(mousePosition);
        float size = Vector3.Distance(mouseScreenPoint, screenPoint) * 2f / EngineManager.Instance.CanvasScale;
        _mouseRotateRect.sizeDelta = new Vector2(size, size);
    }
}