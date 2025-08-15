using UnityEngine;

public class SaturationPicker : ColorPropertyPicker
{
    public float Hue => ColorPickerController.Instance.Hue;
    public float Saturation => ColorPickerController.Instance.Saturation;
    public float Value => ColorPickerController.Instance.Value;

    private void OnEnable()
    {
        ColorPickerController.Instance.OnColorUpdated += OnColorUpdated;
    }

    private void OnDisable()
    {
        if (ColorPickerController.Instance == null) return;
        ColorPickerController.Instance.OnColorUpdated -= OnColorUpdated;
    }

    private void OnColorUpdated(float hue, float saturation, float value, float alpha)
    {
        if (_value == saturation)
        {
            return;
        }
        UpdateTexture();
        _slider.SetValueWithoutNotify(saturation * _range);
        _input.text = (value * _range).ToString();
    }

    protected override void UpdateTexture()
    {
        for (int i = 0; i < _texture.width; i++)
        {
            _texture.SetPixel(i, 0, Color.HSVToRGB(Hue, (float)i / _texture.width, Value));
        }

        _texture.Apply();
    }

    protected override void UpdateColor()
    {
        ColorPickerController.Instance.Saturation = _value;
    }

    public override void Init()
    {
        _slider.SetValueWithoutNotify(Saturation * _range);
        _input.text = (Saturation * _range).ToString();
    }
}