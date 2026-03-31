using System.Collections.Generic;
using System.Linq;

namespace Loykas.Scripting
{
    public static class DataTypeController
    {
        public static ScriptDataType[] DataTypeList = new ScriptDataType[]
        {
            ScriptDataType.Single(DataType.String),
            ScriptDataType.Single(DataType.Number),
            ScriptDataType.Single(DataType.Boolean),
            ScriptDataType.Single(DataType.Color),
            ScriptDataType.Single(DataType.Entity),

            ScriptDataType.List(DataType.String),
            ScriptDataType.List(DataType.Number),
            ScriptDataType.List(DataType.Boolean),
            ScriptDataType.List(DataType.Color),
            ScriptDataType.List(DataType.Entity),
        };

        public static List<string> DataTypesDropdownValues = DataTypeList.Select(s => s.ToString()).ToList();

        public static int TypeToDropdownIndex(ScriptDataType type)
        {
            int index = type.Type switch
            {
                DataType.String => 0,
                DataType.Number => 1,
                DataType.Boolean => 2,
                DataType.Color => 3,
                DataType.Entity => 4,
            };

            if (type.Kind == DataKind.List)
            {
                index += 5;
            }

            return index;
        }
    }   
}