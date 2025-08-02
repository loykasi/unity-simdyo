using UnityEngine;
using UnityEngine.UI;

public class BooleanInputField : VariableInput
{
    [SerializeField] private Toggle _valueToggleField;
    private bool _value;

    private void Awake()
    {
        _valueToggleField.onValueChanged.AddListener(OnValueToggleChanged);
    }

    private void OnValueToggleChanged(bool value)
    {
        _value = value;
        OnValueUpdated?.Invoke();
    }

    public override void SetValue(object value)
    {
        _value = (bool)value;
        _valueToggleField.isOn = _value;
    }

    public override void Enable()
    {
        gameObject.SetActive(true);
        _valueToggleField.isOn = false;
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