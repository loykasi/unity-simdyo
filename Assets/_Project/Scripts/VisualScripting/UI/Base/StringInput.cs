using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StringInput : MonoBehaviour
{
    public event UnityAction<string> OnSubmit;
    public event UnityAction OnValueUpdated;

    public RectTransform Rect;
    public TMP_InputField InputField;
    public float MinWidth = 50f;
    public float MaxWidth = 200f;

    private readonly float _horizontalPadding = 20f;

    private void Awake()
    {
        InputField.onValueChanged.AddListener(OnValueChanged);
        InputField.onEndEdit.AddListener(OnEndEdit);
    }

    private void OnEndEdit(string value)
    {
        OnSubmit?.Invoke(value);
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

    public void SetValue(string value)
    {
        InputField.SetTextWithoutNotify(value);
    }

    public string GetValue()
    {
        return InputField.text;
    }
}