using Loykas.Scripting;

public class ScriptNodeValueData
{
    public string Key;
    public object Value;
    public DataType Type;

    public ScriptNodeValueData() { }

    public ScriptNodeValueData(string key, object value, DataType type)
    {
        Key = key;
        Value = value;
        Type = type;
    }
}