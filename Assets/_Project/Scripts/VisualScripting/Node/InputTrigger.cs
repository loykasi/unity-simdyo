using System;

public class InputTrigger : Port<OutputTrigger>
{
    public Func<VisualScripting, OutputTrigger> Action;

    public OutputTrigger Source;

    public InputTrigger(Func<VisualScripting, OutputTrigger> action)
    {
        Action = action;
    }

    public override void Connect(OutputTrigger port)
    {
        Source = port;
    }

    public void Invoke(VisualScripting vs)
    {
        OutputTrigger output = Action?.Invoke(vs);
        output?.Invoke(vs);
    }

    protected override void DisconnectPort(OutputTrigger port)
    {
        Source = null;
    }
}