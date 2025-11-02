using System;
using UnityEngine;

public class SceneMenuController : MonoBehaviour
{
    [SerializeField] private SceneMenu _menu;
    [SerializeField] private SceneCameraArea _sceneCameraArea;
    private Camera _editorCamera => EngineManager.Instance.EditorCamera;
    private Camera _sceneCamera => EngineManager.Instance.SceneCamera;

    public CameraSettings Settings => SceneManager.Instance.CameraSettings;

    private void Start()
    {
        _menu.UpdateMenu(Settings);
    }

    private void OnEnable()
    {
        ObjectManager.Instance.OnObjectSelected += OnObjectSelected;
        ObjectManager.Instance.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnObjectDeselected()
    {
        _menu.gameObject.SetActive(true);
        _menu.UpdateMenu(Settings);
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

    public void UpdatePosition(Vector2 position)
    {
        Vector3 cameraPosition = new Vector3(position.x, position.y);
        SceneManager.Instance.UpdatePosition(cameraPosition);
        _sceneCameraArea.transform.position = cameraPosition;
    }

    public void UpdateSize(float value)
    {
        SceneManager.Instance.UpdateSize(value);

        float height = Settings.Size * 2f;
        float width = height * _sceneCamera.aspect;
        _sceneCameraArea.SetSize(new Vector2(width, height));
    }

    public void OpenColorEdit()
    {
        ColorPickerController.Instance.Open(Settings.Color, OnColorUpdated);
    }

    private void OnColorUpdated(ColorHSV color)
    {
        SceneManager.Instance.UpdateColor(color);
        _menu.UpdateMenu(Settings);

        _editorCamera.backgroundColor = color.ToUnityColor();
    }

    public void OpenGlobalScript()
    {
        ScriptGraph.Instance.ToggleGlobalScriptPanel();
    }

    public void LoadData(GameData data)
    {
        UpdatePosition(data.Scene.CameraPosition);
        UpdateSize(data.Scene.CameraSize);
        OnColorUpdated(data.Scene.BackgroundColor);
    }
}