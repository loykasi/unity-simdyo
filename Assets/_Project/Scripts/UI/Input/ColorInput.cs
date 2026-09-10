using Loykas.Scripting;
using UnityEngine;
using UnityEngine.UI;

public class ColorInput : BaseInput
{
    public Button Button;
    public Image ButtonImage;
    [SerializeField] private GameObject UnknowStateObject;

    private ColorHSV _value;
    private Color _unknownColor = Color.gray;

    private void Awake()
    {
        Button.onClick.AddListener(OnClick);
        SetValue(new ColorHSV(0f, 0f, 1f, 1f));
    }

    private void OnClick()
    {
        ColorPickerController.Instance.Open(_value, OnColorUpdated);
    }

    private void OnColorUpdated(ColorHSV color)
    {
        UnknowStateObject.SetActive(false);

        _value = color;

        if (ValueInstance != null)
        {
            ValueHandler.SetValue(ValueInstance, _value);
        }

        UpdateButton();
        OnSubmit?.Invoke(_value);
    }

    public override object GetValue()
    {
        return _value;
    }

    public override void SetValueInstance(Variable value)
    {
        base.SetValueInstance(value);
        _value = (ColorHSV)value.Value;
        UpdateButton();
    }

    public override void SetValue(object value)
    {
        UnknowStateObject.SetActive(false);
        _value = (ColorHSV)value;
        UpdateButton();
    }

    private void UpdateButton()
    {
        Color buttonColor = _value.ToUnityColor();
        ButtonImage.color = buttonColor;
    }

    public override void SetDefaultValue(InputValue inputValue)
    {
        inputValue.Value = ValueTransfer.CreateColor(_value);
    }

    public override void SetUnknownState()
    {
        UnknowStateObject.SetActive(true);
        ButtonImage.color = _unknownColor;
    }
}