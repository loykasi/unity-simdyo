using UnityEngine.UI;

public class BooleanInput : BaseInput
{
    public Toggle Input;

    private void Awake()
    {
        Input.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool value)
    {
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

    public override void SetValue(object value)
    {
        Input.SetIsOnWithoutNotify((bool)value);
    }
}