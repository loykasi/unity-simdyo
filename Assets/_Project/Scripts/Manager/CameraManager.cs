using System;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public Action<Color> OnColorChanged = delegate { };

    [SerializeField] private Camera _editorCamera;
    [SerializeField] private Camera _sceneCamera;
    [SerializeField] private Color _defaultBackground;

    public Camera Current => _isGameMode || !_editorCamera ? _sceneCamera : _editorCamera;
    public Camera EditorCamera => _editorCamera;
    public Camera SceneCamera => _sceneCamera;
    public Color Color => Current.backgroundColor;
    public float EditorCameraHeight { get; set; } = 5f;

    public CameraSettings CameraSettings = new();
    private CameraSettings _playSnapshot = new();

    private bool _isGameMode;

    protected override void Awake()
    {
        base.Awake();
        _isGameMode = _editorCamera == null;

        CameraSettings.Position = _sceneCamera.transform.position;
        CameraSettings.Size = _sceneCamera.orthographicSize;
        CameraSettings.Color = new(_defaultBackground);
    }

    public void EnterPlay()
    {
        CaptureSnapshot(_sceneCamera, _playSnapshot);
        _isGameMode = true;

        if (_editorCamera) _editorCamera.gameObject.SetActive(false);
        _sceneCamera.gameObject.SetActive(true);
    }

    public void ExitPlay()
    {
        RestoreSnapshot(_sceneCamera, _playSnapshot);

        if (_editorCamera) _editorCamera.gameObject.SetActive(true);
        _sceneCamera.gameObject.SetActive(false);

        _isGameMode = false;
    }

    public void RestartPlay()
    {
        RestoreSnapshot(_sceneCamera, _playSnapshot);
    }

    public void ResetToDefault()
    {
        _editorCamera.transform.position = new Vector3(0f, 0f, _editorCamera.transform.position.z);
        _editorCamera.orthographicSize = 5f;
        EditorCameraHeight = 5f;
    }

    public void UpdatePosition(Vector3 position)
    {
        CameraSettings.Position = new Vector3(position.x, position.y, _sceneCamera.transform.position.z);
        _sceneCamera.transform.position = CameraSettings.Position;
    }

    public void UpdateSize(float value)
    {
        CameraSettings.Size = value;
        _sceneCamera.orthographicSize = CameraSettings.Size;
    }

    public void UpdateColor(ColorHSV color)
    {
        CameraSettings.Color = color;
        Color unityColor = color.ToUnityColor();
        _editorCamera.backgroundColor = unityColor;
        if (_editorCamera) _sceneCamera.backgroundColor = unityColor;
        OnColorChanged.Invoke(unityColor);
    }

    private void CaptureSnapshot(Camera camera, CameraSettings snapshot)
    {
        snapshot.Color = new ColorHSV(camera.backgroundColor);
        snapshot.Position = camera.transform.position;
        snapshot.Size = camera.orthographicSize;
    }

    private void RestoreSnapshot(Camera camera, CameraSettings snapshot)
    {
        camera.backgroundColor = snapshot.Color.ToUnityColor();
        camera.transform.position = snapshot.Position;
        camera.orthographicSize = snapshot.Size;
    }
}