namespace Loykas.Scripting
{
    [System.Serializable]
    public struct ScriptDataType
    {
        public DataType Type;
        public DataKind Kind;
        public readonly bool IsList => Kind == DataKind.List;

        public readonly bool IsAny => Type == DataType.Any;
        public readonly bool IsAnyKind => Kind == DataKind.Any;

        public ScriptDataType(DataType type, DataKind kind)
        {
            Type = type;
            Kind = kind;
        }

        public override readonly string ToString()
        {
            if (Kind == DataKind.List)
            {
                return $"List | {Type}";
            }

            return Type.ToString();
        }

        public static ScriptDataType Default()
        {
            return new ScriptDataType(DataType.String, DataKind.Simple);
        }

        public static ScriptDataType Any()
        {
            return new ScriptDataType(DataType.Any, DataKind.Any);
        }

        public static ScriptDataType Single(DataType type)
        {
            return new ScriptDataType(type, DataKind.Simple);
        }

        public static ScriptDataType List(DataType type)
        {
            return new ScriptDataType(type, DataKind.List);
        }

        public override readonly int GetHashCode()
        {
            return System.HashCode.Combine(Type, Kind);
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

            return Type == scriptDataType.Type && Kind == scriptDataType.Kind;
        }

        public static bool operator ==(ScriptDataType a, ScriptDataType b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ScriptDataType a, ScriptDataType b)
        {
            return !a.Equals(b);
        }

        public static bool IsCompatible(ScriptDataType a, ScriptDataType b)
        {
            return (a.IsAnyKind || b.IsAnyKind || a.Kind == b.Kind) &&
            (a.IsAny || b.IsAny || a == b);
        }
    }
}