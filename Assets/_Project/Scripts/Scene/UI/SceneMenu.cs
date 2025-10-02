using UnityEngine;
using UnityEngine.UI;

public class SceneMenu : MonoBehaviour
{
    [SerializeField] private SceneMenuController _controller;
    [SerializeField] private Image _bgButtonColor;
    [SerializeField] private MenuVectorInput _positionInput;
    [SerializeField] private MenuNumberInput _sizeInput;

    private void Awake()
    {
        _positionInput.OnSubmit += OnPositionSubmit;
        _sizeInput.OnSubmit += OnSizeSubmit;
    }

    public void UpdateMenu(CameraSettings settings)
    {
        _bgButtonColor.color = settings.Color.ToUnityColor();

        _positionInput.SetValue(settings.Position);
        _sizeInput.SetValue(settings.Size);
    }

    public void OpenColorEdit()
    {
        _controller.OpenColorEdit();
    }

    public void OpenGlobalScript()
    {
        _controller.OpenGlobalScript();
    }

    private void OnSizeSubmit(float value)
    {
        _controller.UpdateSize(value);
    }

    private void OnPositionSubmit(Vector3 value)
    {
        _controller.UpdatePosition(value.x, value.y);
    }
}