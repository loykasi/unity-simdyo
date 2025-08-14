using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValuePicker : ColorPropertyPicker
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
        if (_value == value)
        {
            return;
        }
        UpdateTexture();
        _slider.SetValueWithoutNotify(value * _range);
    }

    protected override void UpdateTexture()
    {
        for (int i = 0; i < _texture.width; i++)
        {
            _texture.SetPixel(i, 0, Color.HSVToRGB(Hue, Saturation, (float)i / _texture.width));
        }

        _texture.Apply();
    }

    protected override void UpdateColor()
    {
        ColorPickerController.Instance.Value = _value;
    }

    public override void Init()
    {
        _slider.SetValueWithoutNotify(Value * _range);
        _input.text = (Value * _range).ToString();
    }
}