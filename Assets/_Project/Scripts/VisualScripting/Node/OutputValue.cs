using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    public class OutputValue : Port<InputValue>
    {
        public ScriptDataType Type { get; private set; }
        public Func<ScriptFlow, object> action;
        public List<InputValue> Destinations = new();

        public OutputValue(string key, Func<ScriptFlow, object> getValue) : base(key)
        {
            action = getValue;
            Type = ScriptDataType.Any();
        }

        public OutputValue(string key, Func<ScriptFlow, object> getValue, ScriptDataType type) : base(key)
        {
            action = getValue;
            Type = type;
        }

        public OutputValue HideLabel()
        {
            ShouldShowLabel = false;
            return this;
        }

        public object GetValue(ScriptFlow vs)
        {
            return action(vs);
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
            return port.Type.IsList == Type.IsList && (Type.IsAny || port.Type.IsAny || port.Type == Type);
        }
    }
}