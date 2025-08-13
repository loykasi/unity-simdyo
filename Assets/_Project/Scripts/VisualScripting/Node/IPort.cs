using System;

public interface IPort
{
    IScriptNode Node { get; set; }
    string Key { get; set; }
    bool ShouldShowLabel { get; set; }

    bool CanConnect(IPort port);
    bool ConnectToPort(IPort port);
    void Disconnect(IPort other);
}