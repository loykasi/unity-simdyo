using System;

namespace Loykas.Scripting
{
    public interface IPort
    {
        IScriptNode Node { get; set; }
        string Key { get; set; }
        bool ShouldShowLabel { get; set; }
        bool ShouldLocalized { get; set; }

        bool CanConnect(IPort port);
        bool ConnectToPort(IPort port);
        void Disconnect(IPort other);
    }
}