using System;
using System.Collections.Generic;
using UnityEngine;

public class ValueOutput : Port<ValueInput>
{
    public Variable Type { get; private set; }
    public Func<object> action;
    public List<ValueInput> Destinations = new();
    public bool IsUseInputField = false;
    private object _value;

    public ValueOutput(Func<object> getValue)
    {
        action = getValue;
        Type = Variable.Any;
    }

    public ValueOutput(Func<object> getValue, Variable type)
    {
        action = getValue;
        Type = type;
    }

    public object GetValue()
    {
        if (IsUseInputField)
        {
            switch (Type)
            {
                case Variable.String:
                    return (string)_value;
                case Variable.Number:
                    return (double)_value;
                case Variable.Boolean:
                    return (bool)_value;
            }
        }

        return action();
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
        else
        {
            Debug.Log("Save as string");
            _value = value;
        }
    }

    public override void Connect(ValueInput port)
    {
        Destinations.Add(port);
    }

    protected override void DisconnectPort(ValueInput port)
    {
        if (!Destinations.Contains(port))
        {
            Destinations.Remove(port);
        }
    }

    public override bool CanConnectTo(ValueInput port)
    {
        Debug.Log($"{GetType()} | {Type} | {port.Type}");
        return port.Type == Variable.Any || port.Type == Type;
    }
}