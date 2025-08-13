using System;
using UnityEngine;

public class InputValue : Port<OutputValue>
{
    public DataType Type { get; set; }
    public OutputValue Source;

    public bool UseOptionalInput;
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

    public InputValue(string key, bool useOptionalInput) : base(key)
    {
        UseOptionalInput = useOptionalInput;
        Type = DataType.Any;
    }

    public InputValue(string key, bool useOptionalInput, DataType type) : base(key)
    {
        UseOptionalInput = useOptionalInput;
        Type = type;
    }

    public void UpdateDefaultValue()
    {
        Debug.Log(Node);
        if (UseOptionalInput)
        {
            if (!HasValue)
            {
                Node.DefaultValues.Add(Key, Type switch
                {
                    DataType.String => "",
                    DataType.Number => 0,
                    DataType.Boolean => false,
                    _ => "",
                });
                return;
            }
            Value = Type switch
            {
                DataType.String => "",
                DataType.Number => 0,
                DataType.Boolean => false,
                _ => "",
            };
        }
    }

    public override void Connect(OutputValue port)
    {
        Source = port;
    }

    public T GetValue<T>(VisualScripting vs)
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

    public object GetValue(VisualScripting vs)
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
        return Type == DataType.Any || port.Type == Type;
    }
}