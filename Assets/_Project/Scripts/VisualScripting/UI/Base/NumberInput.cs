using System;
using TMPro;
using UnityEngine;

public class NumberInput : MonoBehaviour
{
    public RectTransform Rect;
    public TMP_InputField InputField;
    public float MinWidth = 50f;
    public float MaxWidth = 200f;

    private readonly float _horizontalPadding = 20f;

    private void Awake()
    {
        InputField.onValueChanged.AddListener(OnValueChanged);
        InputField.onValidateInput += ValidateInput;
    }

    private char ValidateInput(string text, int charIndex, char addedChar)
    {
        if (char.IsNumber(addedChar))
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
    }

    public string GetValue()
    {
        return InputField.text;
    }
}