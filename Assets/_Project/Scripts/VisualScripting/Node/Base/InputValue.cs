using UnityEngine;
using UnityEngine.Events;

namespace Loykas.Scripting
{
    public enum InputValueTypes
    {
        None,
        String,
        Number,
        Boolean,
        Color,
        Entity,
        Variable,
        GlobalVariable,
        Key,
        CollisionLayer,
        SignalEntity
    }

    public class InputValue : Port<OutputValue>
    {
        public UnityAction OnValueChanged;

        public ScriptDataType Type { get; set; }
        public OutputValue Source;
        public bool HasConnection => Source != null;

        public InputValueTypes InputType = InputValueTypes.None;
        public bool IsNullMeanSelf;

        public bool HasValue => Node.DefaultValues.ContainsKey(Key);
        public ValueTransfer Value
        {
            get
            {
                return Node.DefaultValues[Key];
            }
            set
            {
                Node.DefaultValues[Key] = value;
            }
        }

        public InputValue(IScriptNode node, string key, ScriptDataType type, PortSettings settings) : base(node, key, settings)
        {
            Type = type;
        }

        public void UpdateDefaultValue()
        {
            if (InputType != InputValueTypes.None)
            {
                if (!HasValue)
                {
                    Node.DefaultValues.Add(Key, ValueHandler.GetDefaultValueWrapper(Type));
                    return;
                }

                Value = ValueHandler.GetDefaultValueWrapper(Type);
            }
        }

        public InputValue UseInput()
        {
            InputType = Type.Type switch
            {
                DataType.String => InputValueTypes.String,
                DataType.Number => InputValueTypes.Number,
                DataType.Boolean => InputValueTypes.Boolean,
                DataType.Entity => InputValueTypes.Entity,
                DataType.Color => InputValueTypes.Color,
                _ => InputValueTypes.None
            };
            UpdateDefaultValue();

            return this;
        }

        public InputValue UseInput(InputValueTypes inputType)
        {
            InputType = inputType;
            UpdateDefaultValue();

            return this;
        }

        public InputValue NullMeanSelf()
        {
            IsNullMeanSelf = true;
            return this;
        }

        public ValueTransfer GetValue()
        {
            if (Source != null)
            {
                return Source.GetValue();
            }

            if (HasValue)
            {
                return Value;
            }

            return default;
        }

        public void SetValue(ValueTransfer value)
        {
            Value = value;
            OnValueChanged?.Invoke();
        }

        public void SetType(ScriptDataType type)
        {
            Type = type;
        }

        public override void Connect(OutputValue port)
        {
            if (Source != null)
            {
                Node.Flow.Disconnect(Source, this);
            }
            Source = port;
        }

        protected override void DisconnectPort(OutputValue port)
        {
            if (Source != port)
            {
                return;
            }
            Source = null;
        }

        public override bool CanConnectTo(OutputValue port)
        {            
            return ScriptDataType.IsCompatible(port.Type, Type);
        }
    }
}