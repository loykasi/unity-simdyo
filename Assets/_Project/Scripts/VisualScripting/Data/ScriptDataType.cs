public class ScriptDataType
{
    public string MainType;
    public string SubType;

    public ScriptDataType(string type)
    {
        MainType = type;
    }

    public ScriptDataType(string type, string subType)
    {
        MainType = type;
        SubType = subType;
    }

    public override string ToString()
    {
        if (SubType == null || SubType.Equals(string.Empty))
        {
            return MainType;
        }
        
        return string.Concat(MainType, " of ", SubType);
    }
}