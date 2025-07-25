using System;
using System.Collections.Generic;
using UnityEngine;

public class ValueOutput : Port<ValueInput>
{
    public Variable Type { get; private set; }
    public Func<object> action;
    public List<ValueInput> Destinations = new();

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
        return action();
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