using System;
using System.Collections.Generic;

public class InputTrigger : Port<OutputTrigger>
{
    public Func<VisualScripting, OutputTrigger> Action;

    public List<OutputTrigger> Sources = new();

    public InputTrigger(Func<VisualScripting, OutputTrigger> action)
    {
        Action = action;
    }

    public override void Connect(OutputTrigger port)
    {
        Sources.Add(port);
    }

    public void Invoke(VisualScripting vs)
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