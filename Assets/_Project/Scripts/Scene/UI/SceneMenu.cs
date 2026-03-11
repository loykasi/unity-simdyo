using UnityEngine;
using UnityEngine.UI;

public class SceneMenu : MonoBehaviour
{
    [SerializeField] private Image _bgButtonColor;
    [SerializeField] private MenuVectorInput _positionInput;
    [SerializeField] private MenuNumberInput _sizeInput;

    [SerializeField] private SceneCameraArea _sceneCameraArea;

    private Camera _editorCamera => EngineManager.Instance.EditorCamera;
    private Camera _sceneCamera => EngineManager.Instance.SceneCamera;

    public CameraSettings Settings => SceneManager.Instance.CameraSettings;

    private void Awake()
    {
        _positionInput.OnSubmit += OnPositionSubmit;
        _sizeInput.OnSubmit += OnSizeSubmit;
    }

    private void Start()
    {
        UpdateMenu(Settings);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        UpdateMenu(Settings);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void UpdateMenu(CameraSettings settings)
    {
        _bgButtonColor.color = settings.Color.ToUnityColor();

        _positionInput.SetValue(settings.Position);
        _sizeInput.SetValue(settings.Size);
    }

    public void OpenColorEdit()
    {
        ColorPickerController.Instance.Open(Settings.Color, OnColorUpdated);
    }

    private void OnColorUpdated(ColorHSV color)
    {
        SceneManager.Instance.UpdateColor(color);
        UpdateMenu(Settings);

        _editorCamera.backgroundColor = color.ToUnityColor();
    }

    public void OpenGlobalScript()
    {
        ScriptGraph.Instance.ToggleGlobalScriptPanel();
    }

    private void OnSizeSubmit(float value)
    {
        SceneManager.Instance.UpdateSize(value);

        float height = Settings.Size * 2f;
        float width = height * _sceneCamera.aspect;
        _sceneCameraArea.SetSize(new Vector2(width, height));
    }

    private void OnPositionSubmit(Vector3 value)
    {
        Vector3 cameraPosition = new(value.x, value.y);
        SceneManager.Instance.UpdatePosition(cameraPosition);
        _sceneCameraArea.transform.position = cameraPosition;
    }
}