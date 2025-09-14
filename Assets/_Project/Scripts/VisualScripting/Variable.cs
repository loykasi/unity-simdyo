
public class Variable
{
    public DataType Type;
    public ListType? SubType;

    public object Value;
    private object _default;

    public Variable()
    {
    }

    public Variable(DataType type, object value)
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