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
}

public class InputValue : Port<OutputValue>
{
    public UnityAction OnValueChanged;

    public DataType Type { get; set; }
    public OutputValue Source;

    public InputValueTypes InputType = InputValueTypes.None;

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
        Type = DataType.Any;
    }

    public InputValue(string key, DataType type) : base(key)
    {
        Type = type;
    }

    public void UpdateDefaultValue()
    {
        if (InputType != InputValueTypes.None)
        {
            if (!HasValue)
            {
                Node.DefaultValues.Add(Key, Type switch
                {
                    DataType.String => "",
                    DataType.Number => 0f,
                    DataType.Boolean => false,
                    _ => "",
                });
                return;
            }

            Value = Type switch
            {
                DataType.String => "",
                DataType.Number => 0f,
                DataType.Boolean => false,
                _ => "",
            };
        }
    }

    public InputValue UseInput()
    {
        InputType = Type switch
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
            return Value;
        }

        return null;
    }

    public void SetValue(string value)
    {
        if (Type == DataType.String)
        {
            Debug.Log("Save as string");
            Value = value;
            return;
        }

        if (Type == DataType.Number)
        {
            if (float.TryParse(value, out float result2))
            {
                Debug.Log("Save as float");
                Value = result2;
            }
            return;
        }

        if (Type == DataType.Boolean)
        {
            if (bool.TryParse(value, out bool result3))
            {
                Debug.Log("Save as bool");
                Value = result3;
            }
            return;
        }

        Value = value;
        OnValueChanged?.Invoke();
    }
    
    public void SetValue(float value)
    {
        if (Type == DataType.Number)
        {
            Value = value;
            OnValueChanged?.Invoke();
        }
    }

    public void SetValue(bool value)
    {
        if (Type == DataType.Boolean)
        {
            Value = value;
            OnValueChanged?.Invoke();
        }
    }

    public void SetValue(int index)
    {
        switch (InputType)
        {
            case InputValueTypes.Entity:
                SceneEntity entity = ObjectManager.Instance.GetEntityByIndex(index);
                Value = entity;
                break;
            case InputValueTypes.Variable:
                string variableName = Node.Flow.GetVariableOptions()[index];
                Value = variableName;
                break;
        }

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
        return port.Type == DataType.Any || Type == DataType.Any || port.Type == Type;
    }
}