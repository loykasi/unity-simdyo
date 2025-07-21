public interface IPort
{
    IScriptNode Node { get; set; }

    bool CanConnect(IPort port);
    bool ConnectToPort(IPort port);
    void Disconnect(IPort other);
}