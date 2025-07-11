using System;

public class InputTrigger : Port<OutputTrigger>
{
    public Func<OutputTrigger> Action;

    public OutputTrigger Source;

    public InputTrigger(Func<OutputTrigger> action)
    {
        Action = action;
    }

    public override void Connect(OutputTrigger port)
    {
        Source = port;
    }

    public void Invoke()
    {
        OutputTrigger output = Action?.Invoke();
        output?.Invoke();
    }
}