using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Loykas.Scripting
{
    public class ScriptNode : IScriptNode
    {
        public UnityAction OnNodeUpdated;

        public Guid ID { get; set; }
        public Vector2 Position { get; set; }
        public Dictionary<string, object> DefaultValues { get; set; } = new();

        public ScriptFlow Flow { get; set; }

        public string Title;

        public bool HasInputTrigger => InputTriggers.Count > 0;
        public InputTrigger EnterTrigger => InputTriggers[0];
        public List<InputTrigger> InputTriggers = new();
        public List<OutputTrigger> OutputTriggers = new();
        public List<InputValue> ValueInputs = new();
        public List<OutputValue> ValueOutputs = new();
        public IEnumerable<IPort> Ports()
        {
            foreach (var item in InputTriggers)
            {
                yield return item;
            }
            foreach (var item in OutputTriggers)
            {
                yield return item;
            }
            foreach (var item in ValueInputs)
            {
                yield return item;
            }
            foreach (var item in ValueOutputs)
            {
                yield return item;
            }
        }

        public ScriptNode()
        {
            ID = Guid.NewGuid();
        }

        public ScriptNode(string title)
        {
            ID = Guid.NewGuid();
            Title = title;
        }

        protected InputTrigger InputTrigger(string key, Func<ScriptFlow, OutputTrigger> action)
        {
            InputTrigger inputTrigger = new(key, action)
            {
                Node = this
            };
            InputTriggers.Add(inputTrigger);
            return inputTrigger;
        }

        public OutputTrigger OutputTrigger(string key)
        {
            OutputTrigger outputTrigger = new(key)
            {
                Node = this
            };
            OutputTriggers.Add(outputTrigger);
            return outputTrigger;
        }

        protected InputValue InputValue(string key)
        {
            InputValue valueInput = new(key)
            {
                Node = this
            };
            ValueInputs.Add(valueInput);
            return valueInput;
        }

        protected InputValue InputValue(string key, ScriptDataType type)
        {
            InputValue valueInput = new(key, type)
            {
                Node = this
            };
            ValueInputs.Add(valueInput);
            return valueInput;
        }

        public OutputValue OutputValue(string key, Func<ScriptFlow, object> getValue)
        {
            OutputValue valueOutput = new(key, getValue)
            {
                Node = this
            };
            ValueOutputs.Add(valueOutput);
            return valueOutput;
        }

        public OutputValue OutputValue(string key, ScriptDataType type, Func<ScriptFlow, object> getValue)
        {
            OutputValue valueOutput = new(key, getValue, type)
            {
                Node = this
            };
            ValueOutputs.Add(valueOutput);
            return valueOutput;
        }

        public virtual void UpdateNode()
        {

        }
    }
}