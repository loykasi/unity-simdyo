using UnityEngine.Events;

namespace Loykas.Scripting
{
    public abstract class Port<TOtherPort> : IPort where TOtherPort : IPort
    {
        public UnityAction OnConnected;
        public UnityAction OnDisconnected;

        public IScriptNode Node { get; set; }
        public string Key { get; set; }

        public bool ShouldShowLabel { get; set; } = true;

        public Port(string key)
        {
            Key = key;
        }

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
    }
}