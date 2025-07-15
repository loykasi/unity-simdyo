using System;
using UnityEngine;

public class ValueInput : Port<ValueOutput>
{
    public Type Type { get; private set; }
    public object DefaultValue;
    public ValueOutput Source;

    public bool UseOptionalInput;

    private object _value;

    public ValueInput(bool useOptionalInput)
    {
        UseOptionalInput = useOptionalInput;
    }

    public ValueInput(bool useOptionalInput, Type type)
    {
        UseOptionalInput = useOptionalInput;
        Type = type;
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
        if (Type == typeof(string))
        {
            Debug.Log("Save as string");
            _value = value;
            return;
        }

        if (Type == typeof(double))
        {
            if (int.TryParse(value, out int result1))
            {
                Debug.Log("Save as int");
                _value = result1;
            }
            else if (double.TryParse(value, out double result2))
            {
                Debug.Log("Save as double");
                _value = result2;
            }
            return;
        }

        if (Type == typeof(bool))
        {
            if (bool.TryParse(value, out bool result3))
            {
                Debug.Log("Save as bool");
                _value = result3;
            }
            return;
        }
    }
}