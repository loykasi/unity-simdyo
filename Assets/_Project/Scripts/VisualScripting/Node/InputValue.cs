using UnityEngine;
using UnityEngine.Events;

public enum InputValueTypes
{
    None,
    String,
    Number,
    Boolean,
    Entity,
    Variable,
    Key
}

public class InputValue : Port<OutputValue>
{
    public UnityAction OnValueChanged;

    public ScriptDataType Type { get; set; }
    public OutputValue Source;

    public InputValueTypes InputType = InputValueTypes.None;
    public bool IsDisableConnection;

    public bool HasConnection => Source != null;
    public bool HasValue => Node.DefaultValues.ContainsKey(Key);

    public object Value
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

    public InputValue(string key) : base(key)
    {
        Type = ScriptDataType.Any();
    }

    public InputValue(string key, ScriptDataType type) : base(key)
    {
        Type = type;
    }

    public void UpdateDefaultValue()
    {
        if (InputType != InputValueTypes.None)
        {
            if (!HasValue)
            {
                Node.DefaultValues.Add(Key, ValueHandler.GetDefaultValue(Type));
                return;
            }

            Value = ValueHandler.GetDefaultValue(Type);
        }
    }

    public void SetDefaultValue(object value)
    {
        if (value == null)
        {
            return;
        }

        if (InputType != InputValueTypes.None)
        {
            if (!HasValue)
            {
                Node.DefaultValues.Add(Key, value);
                return;
            }

            Value = value;
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
            _ => InputValueTypes.None
        };
        UpdateDefaultValue();

        return this;
    }

    public InputValue UseVariableInput()
    {
        InputType = InputValueTypes.Variable;
        UpdateDefaultValue();

        return this;
    }

    public InputValue UseKeyCodeInput()
    {
        InputType = InputValueTypes.Key;
        SetDefaultValue(new Loykas.Scripting.Key(Loykas.Scripting.KeyCode.Any));

        return this;
    }

    public InputValue DisableConnection()
    {
        IsDisableConnection = true;
        return this;
    }

    public InputValue HideLabel()
    {
        ShouldShowLabel = false;
        return this;
    }

    public override void Connect(OutputValue port)
    {
        Source = port;
    }

    public T GetValue<T>(ScriptFlow vs)
    {
        if (Source != null)
        {
            return (T)Source.GetValue(vs);
        }

        if (HasValue)
        {
            return (T)Value;
        }

        return default;
    }

    public object GetValue(ScriptFlow vs)
    {
        if (Source != null)
        {
            return Source.GetValue(vs);
        }

        if (HasValue)
        {
            if (Type.Type == DataType.Entity && Value == null)
            {
                return Node.Flow.Entity;    
            }
            return Value;
        }

        return null;
    }

    public void SetValue(object value)
    {
        Value = value;
        OnValueChanged?.Invoke();
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
        return !IsDisableConnection && port.Type.IsList == Type.IsList && (port.Type.IsAny || Type.IsAny || port.Type == Type);
    }
}