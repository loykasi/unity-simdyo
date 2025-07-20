using System;
using System.Collections.Generic;
using UnityEngine;

public class ValueOutput : Port<ValueInput>
{
    public Func<object> action;
    public List<ValueInput> Destinations = new();
    public bool IsUseInputField = false;
    private object _value;

    public ValueOutput(Func<object> getValue)
    {
        action = getValue;
    }

    public ValueOutput()
    {
        IsUseInputField = true;
    }

    public object GetValue()
    {
        if (IsUseInputField)
        {
            return _value;
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
}