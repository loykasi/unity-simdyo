using System;
using UnityEngine;

public class SceneMenuController : MonoBehaviour
{
    [SerializeField] private Color BackgroundColor;
    private CameraSettings _settings = new();

    [SerializeField] private SceneMenu _menu;
    [SerializeField] private SpriteRenderer _sceneCameraArea;
    private Camera _editorCamera => EngineManager.Instance.EditorCamera;
    private Camera _sceneCamera => EngineManager.Instance.SceneCamera;

    private void Awake()
    {
        _settings.Color = new ColorHSV(BackgroundColor);
        _settings.Size = _sceneCamera.orthographicSize;
        _settings.Position = _sceneCamera.transform.position;
        _menu.UpdateMenu(_settings);
    }

    private void OnEnable()
    {
        ObjectManager.Instance.OnObjectSelected += OnObjectSelected;
        ObjectManager.Instance.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnObjectDeselected()
    {
        _menu.gameObject.SetActive(true);
        _menu.UpdateMenu(_settings);
    }

    private void OnObjectSelected(SceneEntity entity)
    {
        _menu.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (ObjectManager.Instance)
        {
            ObjectManager.Instance.OnObjectSelected -= OnObjectSelected;
            ObjectManager.Instance.OnObjectDeselected -= OnObjectDeselected;
        }
    }

    public void UpdatePosition(float x, float y)
    {
        _settings.Position = new Vector3(x, y, _sceneCamera.transform.position.z);
        _sceneCamera.transform.position = _settings.Position;
        _sceneCameraArea.transform.position = new Vector3(x, y);
    }

    public void UpdateSize(float value)
    {
        _settings.Size = value;
        _sceneCamera.orthographicSize = _settings.Size;

        float height = _settings.Size * 2f;
        float width = height * _sceneCamera.aspect;
        _sceneCameraArea.size = new Vector2(width, height);
    }

    public void OpenColorEdit()
    {
        ColorPickerController.Instance.Open(_settings.Color, OnColorUpdated);
    }

    private void OnColorUpdated(ColorHSV color)
    {
        _settings.Color = color;
        BackgroundColor = _settings.Color.ToUnityColor();
        _menu.UpdateMenu(_settings);

        _editorCamera.backgroundColor = _settings.Color.ToUnityColor();
        _sceneCamera.backgroundColor = _settings.Color.ToUnityColor();
    }

    public void OpenGlobalScript()
    {
        ScriptGraph.Instance.ToggleGlobalScriptPanel();
    }
}