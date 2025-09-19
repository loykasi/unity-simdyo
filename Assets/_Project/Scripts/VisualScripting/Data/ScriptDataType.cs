[System.Serializable]
public struct ScriptDataType
{
    public DataType Type;
    public bool IsList;

    public readonly bool IsAny => Type == DataType.Any;

    public ScriptDataType(DataType type, bool isList)
    {
        Type = type;
        IsList = isList;
    }

    public override readonly string ToString()
    {
        if (IsList)
        {
            return $"List | {Type}";
        }

        return Type.ToString();
    }

    public static ScriptDataType Default()
    {
        return new ScriptDataType(DataType.String, false);
    }

    public static ScriptDataType Any()
    {
        return new ScriptDataType(DataType.Any, false);
    }

    public static ScriptDataType Single(DataType type)
    {
        return new ScriptDataType(type, false);
    }

    public static ScriptDataType List(DataType type)
    {
        return new ScriptDataType(type, true);
    }

    public override readonly int GetHashCode()
    {
        return System.HashCode.Combine(Type, IsList);
    }

    public readonly bool Equals(ScriptDataType scriptDataType)
    {
        return base.Equals(scriptDataType);
    }

    public override readonly bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        ScriptDataType scriptDataType = (ScriptDataType)obj;

        return Type == scriptDataType.Type && IsList == scriptDataType.IsList;
    }

    public static bool operator ==(ScriptDataType a, ScriptDataType b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(ScriptDataType a, ScriptDataType b)
    {
        return !a.Equals(b);
    }
}