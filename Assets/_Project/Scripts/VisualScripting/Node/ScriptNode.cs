using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptNode : IScriptNode
{
    public string Title;

    public List<InputTrigger> InputTriggers = new();
    public List<OutputTrigger> OutputTriggers = new();

    public List<ValueInput> ValueInputs = new();
    public List<ValueOutput> ValueOutputs = new();

    public ScriptNode(string title)
    {
        Title = title;
    }

    protected InputTrigger CreateInputTrigger(Func<VisualScripting, OutputTrigger> action)
    {
        InputTrigger inputTrigger = new(action)
        {
            Node = this
        };
        InputTriggers.Add(inputTrigger);
        return inputTrigger;
    }

    protected OutputTrigger CreateOutputTrigger()
    {
        OutputTrigger outputTrigger = new()
        {
            Node = this
        };
        OutputTriggers.Add(outputTrigger);
        return outputTrigger;
    }

    protected ValueInput ValueInput()
    {
        ValueInput valueInput = new(false)
        {
            Node = this
        };
        ValueInputs.Add(valueInput);
        return valueInput;
    }

    protected ValueInput ValueInput(bool useOptionalInput)
    {
        ValueInput valueInput = new(useOptionalInput)
        {
            Node = this
        };
        ValueInputs.Add(valueInput);
        return valueInput;
    }

    protected ValueInput ValueInput<T>(bool useOptionalInput)
    {
        ValueInput valueInput = new(useOptionalInput, typeof(T))
        {
            Node = this
        };
        ValueInputs.Add(valueInput);
        return valueInput;
    }

    protected ValueOutput ValueOutput(Func<object> getValue)
    {
        ValueOutput valueOutput = new(getValue)
        {
            Node = this
        };
        ValueOutputs.Add(valueOutput);
        return valueOutput;
    }
    
    protected ValueOutput ValueOutput()
    {
        ValueOutput valueOutput = new()
        {
            Node = this
        };
        ValueOutputs.Add(valueOutput);
        return valueOutput;
    }
}