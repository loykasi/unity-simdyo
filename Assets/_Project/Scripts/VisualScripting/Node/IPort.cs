public interface IPort
{
    IScriptNode Node { get; set; }

    bool CanConnectTo(IPort port);
    bool ConnectToPort(IPort port);
}