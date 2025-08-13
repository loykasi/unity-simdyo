using System;
using System.Collections.Generic;
using UnityEngine;

public class OutputValue : Port<InputValue>
{
    public DataType Type { get; private set; }
    public Func<VisualScripting, object> action;
    public List<InputValue> Destinations = new();

    public OutputValue(string key, Func<VisualScripting, object> getValue): base(key)
    {
        action = getValue;
        Type = DataType.Any;
    }

    public OutputValue(string key, Func<VisualScripting, object> getValue, DataType type): base(key)
    {
        action = getValue;
        Type = type;
    }

    public object GetValue(VisualScripting vs)
    {
        return action(vs);
    }

    public override void Connect(InputValue port)
    {
        Destinations.Add(port);
    }

    protected override void DisconnectPort(InputValue port)
    {
        if (!Destinations.Contains(port))
        {
            Destinations.Remove(port);
        }
    }

    public override bool CanConnectTo(InputValue port)
    {
        return port.Type == DataType.Any || port.Type == Type;
    }
}