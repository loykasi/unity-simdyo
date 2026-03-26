using UnityEngine.Events;

namespace Loykas.Scripting
{
    public abstract class Port<TOtherPort> : IPort where TOtherPort : IPort
    {
        public UnityAction OnConnected;
        public UnityAction OnDisconnected;

        public IScriptNode Node { get; set; }
        public string Key { get; set; }

        public bool ShowLabel { get; set; }
        public bool IsConnectionDisabled { get; set; }
        public bool UseLocalization { get; set; }
        public string LocalizationKey { get; set; }

        public Port(IScriptNode node, string key)
        {
            Node = node;
            Key = key;
        }

        public Port(IScriptNode node, string key, PortSettings settings)
        {
            Node = node;
            Key = key;

            ShowLabel = !settings.HideLabel;
            IsConnectionDisabled = settings.IsConnectionDisabled;
            UseLocalization = !settings.IsLocalizationDisabled;

            if (string.IsNullOrWhiteSpace(settings.LocalizationKey))
            {
                LocalizationKey = Node.GetNameKey() + "." + Key;
            }
            else
            {
                LocalizationKey = settings.LocalizationKey;
            }
        }

        public virtual bool CanConnect(IPort port)
        {
            return !IsConnectionDisabled && Node != port.Node && port is TOtherPort other && CanConnectTo(other);
        }

        public abstract bool CanConnectTo(TOtherPort port);

        public bool ConnectToPort(IPort port)
        {
            if (port is not TOtherPort)
            {
                return false;
            }

            Connect((TOtherPort)port);
            OnConnected?.Invoke();
            return true;
        }

        public abstract void Connect(TOtherPort port);

        public void Disconnect(IPort port)
        {
            if (port is TOtherPort other)
            {
                DisconnectPort(other);
                OnDisconnected?.Invoke();
            }
        }

        protected abstract void DisconnectPort(TOtherPort port);

        // public string GetLocalizedKey()
        // {
        //     if (ShouldLocalizedPerNode)
        //     {
        //         string name = Node.GetNameKey();
        //         return name + "." + Key;
        //     }

        //     return Key;
        // }
    }
}