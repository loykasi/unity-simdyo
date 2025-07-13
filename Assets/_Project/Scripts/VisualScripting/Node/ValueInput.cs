using UnityEngine;

public class ValueInput : Port<ValueOutput>
{
    public object DefaultValue;
    public ValueOutput Source;

    public bool UseOptionalInput;

    private object _value;

    public ValueInput(bool useOptionalInput)
    {
        UseOptionalInput = useOptionalInput;
    }

    public override void Connect(ValueOutput port)
    {
        Source = port;
    }

    public T GetValue<T>()
    {
        if (Source != null)
        {
            return (T)Source.GetValue();
        }

        return (T)DefaultValue;
    }

    public object GetValue()
    {
        if (Source != null)
        {
            return Source.GetValue();
        }

        if (UseOptionalInput)
        {
            return _value;
        }

        return DefaultValue;
    }
    
    public void SetValue(string value)
    {
        if (int.TryParse(value, out int result1))
        {
            Debug.Log("Save as int");
            _value = result1;
        }
        else if (float.TryParse(value, out float result2))
        {
            Debug.Log("Save as float");
            _value = result2;
        }
    }
}