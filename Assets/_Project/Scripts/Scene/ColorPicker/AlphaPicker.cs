using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlphaPicker : ColorPropertyPicker
{
    public float Hue => ColorPickerController.Instance.Hue;
    public float Saturation => ColorPickerController.Instance.Saturation;
    public float Value => ColorPickerController.Instance.Value;
    public float Alpha => ColorPickerController.Instance.Alpha;

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
        if (_value == alpha)
        {
            return;
        }
        UpdateTexture();
        _slider.SetValueWithoutNotify(alpha * _range);
    }

    protected override void UpdateTexture()
    {
        for (int i = 0; i < _texture.width; i++)
        {
            Color color = Color.HSVToRGB(Hue, Saturation, Value);
            color.a = (float)i / _texture.width;
            _texture.SetPixel(i, 0, color);
        }

        _texture.Apply();
    }

    protected override void UpdateColor()
    {
        ColorPickerController.Instance.Alpha = _value;
    }

    public override void Init()
    {
        _slider.SetValueWithoutNotify(Alpha * _range);
        _input.text = (Alpha * _range).ToString();
    }
}