using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class InputTrigger : Port<OutputTrigger>
{
    public Func<ScriptFlow, OutputTrigger> Action;
    public List<OutputTrigger> Sources = new();

    public InputTrigger(string key, Func<ScriptFlow, OutputTrigger> action) : base(key)
    {
        Action = action;
    }

    public override bool CanConnectTo(OutputTrigger port)
    {
        return true;
    }

    public override void Connect(OutputTrigger port)
    {
        Sources.Add(port);
    }

    public void Invoke(ScriptFlow vs)
    {
        OutputTrigger output = Action?.Invoke(vs);
        output?.Invoke(vs);
    }

    protected override void DisconnectPort(OutputTrigger port)
    {
        if (!Sources.Contains(port))
        {
            Sources.Remove(port);
        }
    }
}