using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneMenu : MonoBehaviour
{
    [SerializeField] private SceneMenuController _controller;
    [SerializeField] private Image _bgButtonColor;
    [SerializeField] private TMP_InputField _positionXInput;
    [SerializeField] private TMP_InputField _positionYInput;
    [SerializeField] private TMP_InputField _sizeInput;
    private CameraSettings _settings;

    public void UpdateMenu(CameraSettings settings)
    {
        _settings = settings;
        _bgButtonColor.color = settings.Color.ToUnityColor();

        _positionXInput.SetTextWithoutNotify(settings.Position.x.ToString());
        _positionYInput.SetTextWithoutNotify(settings.Position.y.ToString());

        _sizeInput.SetTextWithoutNotify(settings.Size.ToString());
    }

    public void OpenColorEdit()
    {
        _controller.OpenColorEdit();
    }

    public void OnEditX(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _positionXInput.SetTextWithoutNotify(result.ToString());
        }
        else
        {
            _positionXInput.SetTextWithoutNotify(_settings.Position.x.ToString());
            result = _settings.Position.x;
        }
        _controller.UpdatePosition(result, _settings.Position.y);
    }

    public void OnEditY(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _positionYInput.SetTextWithoutNotify(result.ToString());
        }
        else
        {
            _positionYInput.SetTextWithoutNotify(_settings.Position.y.ToString());
            result = _settings.Position.y;
        }
        _controller.UpdatePosition(_settings.Position.x, result);
    }

    public void OnEditSize(string value)
    {
        if (float.TryParse(value, out float result))
        {
            _positionYInput.SetTextWithoutNotify(result.ToString());
        }
        else
        {
            _positionYInput.SetTextWithoutNotify(_settings.Size.ToString());
            result = _settings.Size;
        }
        _controller.UpdateSize(result);
    }
}