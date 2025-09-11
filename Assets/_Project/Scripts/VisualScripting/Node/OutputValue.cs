using System;
using System.Collections.Generic;
using UnityEngine;

public class OutputValue : Port<InputValue>
{
    public DataType Type { get; private set; }
    public Func<ScriptFlow, object> action;
    public List<InputValue> Destinations = new();

    public OutputValue(string key, Func<ScriptFlow, object> getValue): base(key)
    {
        action = getValue;
        Type = DataType.Any;
    }

    public OutputValue(string key, Func<ScriptFlow, object> getValue, DataType type): base(key)
    {
        action = getValue;
        Type = type;
    }

    public object GetValue(ScriptFlow vs)
    {
        return action(vs);
    }

    public void SetType(DataType type)
    {
        Type = type;
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
        return Type == DataType.Any || port.Type == DataType.Any || port.Type == Type;
    }
}