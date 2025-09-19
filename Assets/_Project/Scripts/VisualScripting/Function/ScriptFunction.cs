using System.Collections.Generic;

public class ScriptFunction
{
    public string Name;

    public ScriptNode StartNode;
    public ScriptNode ReturnNode;
    public ScriptNode CallNode;

    public List<FunctionInput> Inputs = new();

    public bool HasReturnValue;
    public DataType ReturnType = DataType.String;

    public void AddInput()
    {
        FunctionInput input = new();
        Inputs.Add(input);

        // StartNode.OutputValue("output");
    }

    public void EditInput(int index, string name, DataType type)
    {
        FunctionInput input = Inputs[index];
        input.Name = name;
        input.Type = type;
    }

    public void SetReturnValue(bool value)
    {
        HasReturnValue = value;
    }

    public void EditReturnValue(DataType type)
    {
        ReturnType = type;
    }
}