using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loykas.Scripting
{
    public class DataTypeController : Singleton<DataTypeController>
    {
        public List<DataType> DataTypes = new();

        public static ScriptDataType[] DataTypeList = new ScriptDataType[]
        {
            new(DataType.String, false),
            new(DataType.Number, false),
            new(DataType.Boolean, false),
            new(DataType.Color, false),
            new(DataType.Entity, false),

            new(DataType.String, true),
            new(DataType.Number, true),
            new(DataType.Boolean, true),
            new(DataType.Color, true),
            new(DataType.Entity, true),
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

            if (type.IsList)
            {
                index += 5;
            }

            return index;
        }
    }   
}