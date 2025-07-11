public abstract class Port<TOtherPort> : IPort where TOtherPort : IPort
{
    public IScriptNode Node { get; set; }

    public bool CanConnectTo(IPort port)
    {
        return Node != port.Node && port is TOtherPort;
    }

    public bool ConnectToPort(IPort port)
    {
        if (port is not TOtherPort)
        {
            return false;
        }

        Connect((TOtherPort)port);
        return true;
    }
    
    public abstract void Connect(TOtherPort port);
}