
public class Variable
{
    // public DataType Type;
    public ScriptDataType Type;
    public object Value;
    private object _default;

    public Variable()
    {
    }

    public Variable(DataType type, object value)
    {
        Type = new ScriptDataType(type.ToString());
        Value = value;
    }

    public void OnSceneStart()
    {
        _default = Value;
    }

    public void OnSceneStop()
    {
        Value = _default;
    }
}