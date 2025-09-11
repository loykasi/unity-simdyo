public class OutputTrigger : Port<InputTrigger>
{
    public string Name;
    public InputTrigger Destination;

    public OutputTrigger(string key) : base(key)
    {
    }

    public OutputTrigger HideLabel()
    {
        ShouldShowLabel = false;
        return this;
    }

    public override bool CanConnectTo(InputTrigger port)
    {
        return true;
    }

    public override void Connect(InputTrigger port)
    {
        DisconnectPort(Destination);
        Destination = port;
    }

    public void Invoke(ScriptFlow vs)
    {
        Destination?.Invoke(vs);
    }

    protected override void DisconnectPort(InputTrigger port)
    {
        if (Destination != port)
        {
            return;
        }
        Destination = null;
    }
}