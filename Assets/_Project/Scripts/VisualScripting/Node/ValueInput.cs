using System;
using UnityEngine;

public class ValueInput : Port<ValueOutput>
{
    public Variable Type { get; private set; }
    public object Value => _value;
    public ValueOutput Source;

    public bool UseOptionalInput;

    private object _value;

    public ValueInput(bool useOptionalInput)
    {
        UseOptionalInput = useOptionalInput;
        Type = Variable.Any;
    }

    public ValueInput(bool useOptionalInput, Variable type)
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

        return (T)Value;
    }

    public object GetValue()
    {
        if (Source != null)
        {
            return Source.GetValue();
        }

        return Value;
    }

    public void SetValue(string value)
    {
        if (Type == Variable.String)
        {
            Debug.Log("Save as string");
            _value = value;
            return;
        }

        if (Type == Variable.Number)
        {
            // if (int.TryParse(value, out int result1))
            // {
            //     Debug.Log("Save as int");
            //     _value = result1;
            // }
            if (double.TryParse(value, out double result2))
            {
                Debug.Log("Save as double");
                _value = result2;
            }
            return;
        }

        if (Type == Variable.Boolean)
        {
            if (bool.TryParse(value, out bool result3))
            {
                Debug.Log("Save as bool");
                _value = result3;
            }
            return;
        }
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
        Debug.Log($"{GetType()} | {port.Type} | {Type}");
        return Type == Variable.Any || port.Type == Type;
    }
}