using TMPro;
using UnityEngine;

public class StringInputField : VariableInput
{
    [SerializeField] private TMP_InputField _valueInputField;
    private string _value;

    private void Awake()
    {
        _valueInputField.onEndEdit.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string value)
    {
        _value = value;
        OnValueUpdated?.Invoke();
    }

    public override void SetValue(object value)
    {
        _value = (string)value;
        _valueInputField.text = _value;
    }

    public override void Enable()
    {
        gameObject.SetActive(true);
        _valueInputField.text = "";
    }

    public override void Disable()
    {
        gameObject.SetActive(false);
    }

    public override object GetValue()
    {
        return _value;
    }
}