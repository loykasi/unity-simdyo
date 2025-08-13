using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public abstract class ScriptNode : IScriptNode
{
    public Guid ID { get; set; }
    public Vector2 Positon { get; set; }
    public Dictionary<string, object> DefaultValues { get; set; } = new();

    [JsonIgnore]
    public string Title;

    [JsonIgnore]
    public List<InputTrigger> InputTriggers = new();

    [JsonIgnore]
    public List<OutputTrigger> OutputTriggers = new();

    [JsonIgnore]
    public List<InputValue> ValueInputs = new();

    [JsonIgnore]
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

    public ScriptNode(string title)
    {
        ID = Guid.NewGuid();
        Title = title;
    }

    protected InputTrigger InputTrigger(string key, Func<VisualScripting, OutputTrigger> action)
    {
        InputTrigger inputTrigger = new(key, action)
        {
            Node = this
        };
        InputTriggers.Add(inputTrigger);
        return inputTrigger;
    }

    protected OutputTrigger OutputTrigger(string key)
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
        InputValue valueInput = new(key, false)
        {
            Node = this
        };
        valueInput.UpdateDefaultValue();
        ValueInputs.Add(valueInput);
        return valueInput;
    }

    protected InputValue InputValue(string key, bool useOptionalInput)
    {
        InputValue valueInput = new(key, useOptionalInput)
        {
            Node = this
        };
        valueInput.UpdateDefaultValue();
        ValueInputs.Add(valueInput);
        return valueInput;
    }

    protected InputValue InputValue(string key, DataType type, bool useOptionalInput)
    {
        InputValue valueInput = new(key, useOptionalInput, type)
        {
            Node = this
        };
        valueInput.UpdateDefaultValue();
        ValueInputs.Add(valueInput);
        return valueInput;
    }

    protected OutputValue OutputValue(string key, Func<VisualScripting, object> getValue)
    {
        OutputValue valueOutput = new(key, getValue)
        {
            Node = this
        };
        ValueOutputs.Add(valueOutput);
        return valueOutput;
    }

    protected OutputValue OutputValue(string key, DataType type, Func<VisualScripting, object> getValue)
    {
        OutputValue valueOutput = new(key, getValue, type)
        {
            Node = this
        };
        ValueOutputs.Add(valueOutput);
        return valueOutput;
    }
}