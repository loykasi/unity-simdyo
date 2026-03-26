using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Loykas.Scripting
{
    public class ScriptNode : IScriptNode
    {
        public UnityAction OnNodeUpdated;

        public virtual ScriptNodeCategory Category => default;
        public virtual bool ShouldIncludeInMenu => true;
        public virtual bool CanUseGlobal => true;

        public Guid ID { get; set; }
        public Vector2 Position { get; set; }

        public ScriptFlow Flow
        {
            get => _flow;
            set
            {
                _flow = value;
                FlowAssigned();
            }
        }
        private ScriptFlow _flow;

        public virtual bool ShouldLocalized { get; } = true;

        public InputTrigger InputTrigger;
        public List<OutputTrigger> OutputTriggers = new();
        public List<InputValue> ValueInputs = new();
        public List<OutputValue> ValueOutputs = new();
        public List<IPort> Ports = new();

        public bool HasInputTrigger => InputTrigger != null;
        public bool HasOutputTriggers => OutputTriggers.Count > 0;

        public Dictionary<string, ValueTransfer> DefaultValues { get; set; } = new();

        public virtual ScriptNode Create()
        {
            throw new NotImplementedException();
        }

        public ScriptNode()
        {
            ID = Guid.NewGuid();
        }

        public virtual void FlowAssigned()
        {
            
        }

        public virtual void Init()
        {
            
        }

        public virtual void Reset()
        {
            
        }

        public virtual string GetNameKey()
        {
            return GetType().Name;
        }

        protected InputTrigger CreateInputTrigger(string key, Func<NodeTask, OutputTrigger> action)
        {
            InputTrigger = new
            (
                node: this,
                key: key,
                action: action
            );

            Ports.Add(InputTrigger);
            return InputTrigger;
        }

        public OutputTrigger CreateOutputTrigger(string key)
        {
            return CreateOutputTrigger(key, PortSettings.Default);
        }

        public OutputTrigger CreateOutputTrigger(string key, PortSettings settings)
        {
            OutputTrigger outputTrigger = new
            (
                node: this,
                key: key,
                settings: settings
            );
            
            OutputTriggers.Add(outputTrigger);
            Ports.Add(outputTrigger);
            return outputTrigger;
        }

        protected InputValue CreateInputValue(string key, ScriptDataType type)
        {
            return CreateInputValue(key, type, PortSettings.Default);
        }

        protected InputValue CreateInputValue(string key, ScriptDataType type, PortSettings settings)
        {
            InputValue valueInput = new
            (
                node: this,
                key: key,
                type: type,
                settings: settings
            );

            ValueInputs.Add(valueInput);
            Ports.Add(valueInput);
            return valueInput;
        }

        public OutputValue CreateOutputValue(string key, Func<ValueTransfer> getValue, ScriptDataType type)
        {
            return CreateOutputValue(key, getValue, type, PortSettings.Default);
        }

        public OutputValue CreateOutputValue(string key, Func<ValueTransfer> getValue, ScriptDataType type, PortSettings settings)
        {
            OutputValue valueOutput = new
            (
                node: this,
                key: key,
                getValue: getValue,
                type: type,
                settings: settings
            );

            ValueOutputs.Add(valueOutput);
            Ports.Add(valueOutput);
            return valueOutput;
        }

        public virtual void UpdateNode()
        {

        }

        public void Clear()
        {
            foreach (IPort port in Ports)
            {
                Flow.Disconnect(port);
            }
            ScriptNodeFactory.Instance.ReleaseNode(this);
        }
    }
}