using System;
using System.Collections.Generic;
using UnityEngine;

public class ValueOutput : Port<ValueInput>
{
    public DataType Type { get; private set; }
    public Func<VisualScripting, object> action;
    public List<ValueInput> Destinations = new();

    public ValueOutput(Func<VisualScripting, object> getValue)
    {
        action = getValue;
        Type = DataType.Any;
    }

    public ValueOutput(Func<VisualScripting, object> getValue, DataType type)
    {
        action = getValue;
        Type = type;
    }

    public object GetValue(VisualScripting vs)
    {
        return action(vs);
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
        return port.Type == DataType.Any || port.Type == Type;
    }
}