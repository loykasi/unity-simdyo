
public class Variable
{
    public ScriptDataType Type;

    public object Value;
    private object _default;

    public Variable()
    {
        Type = ScriptDataType.Default();
        Value = string.Empty;
    }

    public Variable(ScriptDataType type, object value)
    {
        Type = type;
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