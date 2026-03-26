using System;

namespace Loykas.Scripting
{
    public interface IPort
    {
        IScriptNode Node { get; set; }
        string Key { get; set; }
        
        bool ShowLabel { get; set; }
        bool IsConnectionDisabled { get; set; }
        bool UseLocalization { get; set; }
        string LocalizationKey { get; set; }

        bool CanConnect(IPort port);
        bool ConnectToPort(IPort port);
        void Disconnect(IPort other);
        // string GetLocalizedKey();
    }
}