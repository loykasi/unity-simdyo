using Loykas.Scripting;
using UnityEngine;
using UnityEngine.UI;

public class BooleanInput : BaseInput
{
    public Toggle Input;
    [SerializeField] private GameObject UnknowStateObject;
    
    private bool _value;

    private void Awake()
    {
        Input.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool value)
    {
        UnknowStateObject.SetActive(false);

        _value = value;
        if (ValueInstance != null)
        {
            ValueHandler.SetValue(ValueInstance, value);
        }

        OnSubmit?.Invoke(value);
    }

    public override object GetValue()
    {
        return Input.isOn;
    }

    public override void SetValueInstance(Variable value)
    {
        base.SetValueInstance(value);
        Input.SetIsOnWithoutNotify((bool)value.Value);
    }

    public override void SetValue(object value)
    {
        Input.SetIsOnWithoutNotify((bool)value);
    }

    public override void SetDefaultValue(InputValue inputValue)
    {
        inputValue.Value = ValueTransfer.CreateBool(_value);
    }

    public override void SetUnknownState()
    {
        UnknowStateObject.SetActive(true);
        Input.SetIsOnWithoutNotify(false);
    }
}