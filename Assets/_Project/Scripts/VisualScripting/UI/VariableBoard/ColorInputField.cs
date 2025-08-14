using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorInputField : VariableInput
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _buttonImage;
    private ColorHSV _colorHSV = new(0f, 0f, 1f, 1f);

    private void Awake()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        ColorPickerController.Instance.Open(_colorHSV, OnColorUpdated);
    }

    private void OnColorUpdated(ColorHSV color)
    {
        _colorHSV = color;

        Color buttonColor = Color.HSVToRGB(_colorHSV.H, _colorHSV.S, _colorHSV.V);
        buttonColor.a = _colorHSV.A;
        _buttonImage.color = buttonColor;

        OnValueUpdated?.Invoke();
    }

    private void UpdateButton()
    {
        Debug.Log($"{_colorHSV.H}, {_colorHSV.S}, {_colorHSV.V}");
        Color buttonColor = Color.HSVToRGB(_colorHSV.H, _colorHSV.S, _colorHSV.V);
        buttonColor.a = _colorHSV.A;
        _buttonImage.color = buttonColor;
    }

    public override void Disable()
    {
        gameObject.SetActive(false);
    }

    public override void Enable()
    {
        gameObject.SetActive(true);
    }

    public override object GetValue()
    {
        return _colorHSV;
    }

    public override void SetValue(object value)
    {
        _colorHSV = (ColorHSV)value;
        UpdateButton();
    }
}