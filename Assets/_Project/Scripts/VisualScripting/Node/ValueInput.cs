using System;
using UnityEngine;

public class ValueInput : Port<ValueOutput>
{
    public DataType Type { get; set; }
    public object Value => _value;
    public ValueOutput Source;

    public bool UseOptionalInput;

    private object _value;

    public ValueInput(bool useOptionalInput)
    {
        UseOptionalInput = useOptionalInput;
        Type = DataType.Any;
    }

    public ValueInput(bool useOptionalInput, DataType type)
    {
        UseOptionalInput = useOptionalInput;
        Type = type;
    }

    public override void Connect(ValueOutput port)
    {
        Source = port;
    }

    public T GetValue<T>(VisualScripting vs)
    {
        if (Source != null)
        {
            return (T)Source.GetValue(vs);
        }

        return (T)Value;
    }

    public object GetValue(VisualScripting vs)
    {
        if (Source != null)
        {
            return Source.GetValue(vs);
        }

        return Value;
    }

    public void SetValue(string value)
    {
        if (Type == DataType.String)
        {
            Debug.Log("Save as string");
            _value = value;
            return;
        }

        if (Type == DataType.Number)
        {
            if (float.TryParse(value, out float result2))
            {
                Debug.Log("Save as float");
                _value = result2;
            }
            return;
        }

        if (Type == DataType.Boolean)
        {
            if (bool.TryParse(value, out bool result3))
            {
                Debug.Log("Save as bool");
                _value = result3;
            }
            return;
        }

        _value = value;
    }

    protected override void DisconnectPort(ValueOutput port)
    {
        if (Source != port)
        {
            return;
        }
        Source = null;
    }

    public override bool CanConnectTo(ValueOutput port)
    {
        return Type == DataType.Any || port.Type == Type;
    }
}