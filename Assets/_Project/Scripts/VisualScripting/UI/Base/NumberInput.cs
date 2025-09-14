using TMPro;
using UnityEngine;

public class NumberInput : BaseInput
{
    public TMP_InputField InputField;
    public float MinWidth = 50f;
    public float MaxWidth = 200f;

    private float _value;

    private readonly float _horizontalPadding = 20f;

    private void Awake()
    {
        InputField.onValueChanged.AddListener(OnValueChanged);
        InputField.onEndEdit.AddListener(OnEndEdit);

        SetValue(0f);
    }

    private void OnValueChanged(string value)
    {
        Vector2 size = InputField.textComponent.GetPreferredValues(value);
        float x = Mathf.Clamp(size.x + _horizontalPadding, MinWidth, MaxWidth);
        Rect.sizeDelta = new Vector2
        (
            x,
            Rect.sizeDelta.y
        );
        OnValueUpdated?.Invoke();
    }

    private void OnEndEdit(string value)
    {
        if (float.TryParse(value, out float parsedValue))
        {
            _value = parsedValue;
        }

        if (ValueInstance != null)
        {
            ValueHandler.SetValue(ValueInstance, _value);
        }

        OnSubmit?.Invoke(_value);
    }

    public override object GetValue()
    {
        return _value;
    }

    public override void SetValue(object value)
    {
        _value = (float)value;
        InputField.SetTextWithoutNotify(_value.ToString());
    }
}