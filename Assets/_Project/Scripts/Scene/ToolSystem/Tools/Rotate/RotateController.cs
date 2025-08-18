using UnityEngine;

public class RotateController : Singleton<RotateController>
{
    [SerializeField] private RectTransform _visualization;
    [SerializeField] private float _snapRadius;

    public float GetSnapRadiusWorld()
    {
        Camera camera = EngineManager.Instance.EditorCamera;
        float scale = camera.orthographicSize * 2 / Screen.height;
        return _snapRadius * scale;
    }

    public void EnableVisualization()
    {
        _visualization.gameObject.SetActive(true);
    }

    public void DisableVisualization()
    {
        _visualization.gameObject.SetActive(false);
    }

    public void UpdateUI(Vector3 worldPosition, Quaternion rotation)
    {
        Camera camera = EngineManager.Instance.EditorCamera;
        Vector3 screenPoint = camera.WorldToScreenPoint(worldPosition);

        _visualization.SetPositionAndRotation(screenPoint, rotation);
        _visualization.sizeDelta = _snapRadius * 2f * Vector2.one;
    }
}