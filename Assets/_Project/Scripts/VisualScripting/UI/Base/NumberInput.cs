using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NumberInput : MonoBehaviour
{
    public event UnityAction<float> OnSubmit;
    public event UnityAction OnValueUpdated;

    public RectTransform Rect;
    public TMP_InputField InputField;
    public float MinWidth = 50f;
    public float MaxWidth = 200f;

    private float _value;

    private readonly float _horizontalPadding = 20f;

    private void Awake()
    {
        InputField.onValueChanged.AddListener(OnValueChanged);
        InputField.onValidateInput += ValidateInput;
        InputField.onEndEdit.AddListener(OnEndEdit);
    }

    public void SetValue(float value)
    {
        _value = value;
        InputField.SetTextWithoutNotify(_value.ToString());
    }

    private char ValidateInput(string text, int charIndex, char addedChar)
    {
        if (char.IsNumber(addedChar) || addedChar == '.' || addedChar == '-' || addedChar == '+')
        {
            return addedChar;
        }

        return '\0';
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
        
        OnSubmit?.Invoke(_value);
    }
}