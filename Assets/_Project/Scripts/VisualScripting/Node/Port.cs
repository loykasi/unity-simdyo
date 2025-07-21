public abstract class Port<TOtherPort> : IPort where TOtherPort : IPort
{
    public IScriptNode Node { get; set; }

    public virtual bool CanConnect(IPort port)
    {
        return Node != port.Node && port is TOtherPort other && CanConnectTo(other);
    }

    public abstract bool CanConnectTo(TOtherPort port);

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

    public void Disconnect(IPort port)
    {
        if (port is TOtherPort other)
        {
            DisconnectPort(other);
        }
    }

    protected abstract void DisconnectPort(TOtherPort port);
}