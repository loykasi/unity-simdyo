using UnityEngine;
using UnityEngine.UI;

public class SceneMenu : MonoBehaviour
{
    [SerializeField] private Image _bgButtonColor;
    [SerializeField] private MenuVectorInput _positionInput;
    [SerializeField] private MenuNumberInput _sizeInput;
    [SerializeField] private SceneCameraArea _sceneCameraArea;

    public CameraSettings Settings => CameraManager.Instance.CameraSettings;

    private void Start()
    {
        _positionInput.OnSubmit += OnPositionSubmit;
        _sizeInput.OnSubmit += OnSizeSubmit;
        
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
        CameraManager.Instance.UpdateColor(color);
        UpdateMenu(Settings);
    }

    public void OpenGlobalScript()
    {
        ScriptGraph.Instance.ToggleGlobalScriptPanel();
    }

    private void OnSizeSubmit(float value)
    {
        CameraManager.Instance.UpdateSize(value);

        float height = Settings.Size * 2f;
        float width = height * CameraManager.Instance.SceneCamera.aspect;
        _sceneCameraArea.SetSize(new Vector2(width, height));
    }

    private void OnPositionSubmit(Vector3 value)
    {
        Vector3 cameraPosition = new(value.x, value.y);
        CameraManager.Instance.UpdatePosition(cameraPosition);
        _sceneCameraArea.transform.position = cameraPosition;
    }
}