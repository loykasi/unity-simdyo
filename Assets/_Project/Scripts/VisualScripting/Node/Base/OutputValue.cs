using System;
using System.Collections.Generic;
using UnityEngine;

namespace Loykas.Scripting
{
    public class OutputValue : Port<InputValue>
    {
        public ScriptDataType Type { get; private set; }
        public Func<object> action;
        public List<InputValue> Destinations = new();

        public OutputValue(string key, Func<object> getValue) : base(key)
        {
            action = getValue;
            Type = ScriptDataType.Any();
        }

        public OutputValue(string key, Func<object> getValue, ScriptDataType type) : base(key)
        {
            action = getValue;
            Type = type;
        }

        public OutputValue HideLabel()
        {
            ShouldShowLabel = false;
            return this;
        }

        public object GetValue()
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

        public OutputValue NoLocalize()
        {
            ShouldLocalized = false;
            return this;
        }

        public OutputValue UseGlobalLocalized()
        {
            ShouldLocalizedPerNode = false;
            return this;
        }
    }
}