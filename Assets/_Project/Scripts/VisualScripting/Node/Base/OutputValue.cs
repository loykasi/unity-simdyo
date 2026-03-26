using System;
using System.Collections.Generic;

namespace Loykas.Scripting
{
    public class OutputValue : Port<InputValue>
    {
        public ScriptDataType Type { get; private set; }
        public Func<ValueTransfer> action;
        public List<InputValue> Destinations = new();

        public OutputValue
        (
            IScriptNode node,
            string key,
            Func<ValueTransfer> getValue,
            ScriptDataType type,
            PortSettings settings
        ) : base(node, key, settings)
        {
            action = getValue;
            Type = type;
        }

        public ValueTransfer GetValue()
        {
            return action();
        }

        public void SetType(ScriptDataType type)
        {
            Type = type;
        }

        public override void Connect(InputValue port)
        {
            Destinations.Add(port);
        }

        protected override void DisconnectPort(InputValue port)
        {
            if (!Destinations.Contains(port))
            {
                Destinations.Remove(port);
            }
        }

        public override bool CanConnectTo(InputValue port)
        {
            return ScriptDataType.IsCompatible(Type, port.Type);
        }
    }
}