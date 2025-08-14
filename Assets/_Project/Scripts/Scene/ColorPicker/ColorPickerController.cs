using UnityEngine;
using UnityEngine.Events;

public class ColorPickerController : Singleton<ColorPickerController>
{
    [SerializeField] private GameObject _window;
    [SerializeField] private ColorProperty[] _colorProperties;

    public UnityAction<float, float, float, float> OnColorUpdated;

    private float _hue = 0f;
    public float Hue
    {
        get => _hue;
        set
        {
            _hue = value;
            ColorUpdated();
        }
    }

    private float _saturation = 1f;
    public float Saturation
    {
        get => _saturation;
        set
        {
            _saturation = value;
            ColorUpdated();
        }
    }

    private float _value = 1f;
    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            ColorUpdated();
        }
    }

    private float _alpha = 1f;
    public float Alpha
    {
        get => _alpha;
        set
        {
            _alpha = value;
            ColorUpdated();
        }
    }

    public void UpdateSaturationAndValue(float saturation, float value)
    {
        Saturation = saturation;
        Value = value;
        OnColorUpdated?.Invoke(Hue, Saturation, Value, Alpha);
    }

    private UnityAction<ColorHSV> _callback;

    public void Open(ColorHSV color, UnityAction<ColorHSV> action)
    {
        _hue = color.H;
        _saturation = color.S;
        _value = color.V;
        _alpha = color.A;

        _callback = action;
        _window.SetActive(true);
        InitColorPicker();
    }

    public void Close()
    {
        _callback = null;
        _window.SetActive(false);
    }

    private void InitColorPicker()
    {
        foreach (var item in _colorProperties)
        {
            item.Init();
        }
    }

    private void ColorUpdated()
    {
        OnColorUpdated?.Invoke(_hue, _saturation, _value, _alpha);
        _callback?.Invoke(new ColorHSV(_hue, _saturation, _value, _alpha));
    }
}