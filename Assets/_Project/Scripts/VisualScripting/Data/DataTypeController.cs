using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loykas.Scripting
{
    public class DataTypeController : Singleton<DataTypeController>
    {
        [Serializable]
        public struct CustomType
        {
            public DataType Type;
            public DataType? SubType;

            public CustomType(DataType type, DataType? subType = null)
            {
                Type = type;
                SubType = subType;
            }

            public override readonly string ToString()
            {
                if (SubType == null)
                {
                    return Type.ToString();
                }
                return string.Concat(Type, " of ", SubType);
            }
        }

        public List<DataType> DataTypes = new();
        // public List<string> DataTypesDropdownValues = new();

        public static CustomType[] DataTypeList = new CustomType[]
        {
            new(DataType.String),
            new(DataType.Number),
            new(DataType.Boolean),
            new(DataType.Color),
            new(DataType.Entity),
            new(DataType.List, DataType.String),
            new(DataType.List, DataType.Number),
            new(DataType.List, DataType.Boolean),
            new(DataType.List, DataType.Color),
        };

        public static List<string> DataTypesDropdownValues = DataTypeList.Select(s => s.ToString()).ToList();

        protected override void Awake()
        {
            base.Awake();

            // InitDataTypes();
        }

        // private void InitDataTypes()
        // {
        //     var defaults = Enum.GetValues(typeof(DataType));

        //     foreach (DataType type in defaults)
        //     {
        //         if (type == DataType.Any)
        //         {
        //             continue;
        //         }

        //         DataTypes.Add(type);

        //         if (type == DataType.List)
        //         {
        //             var listTypes = Enum.GetValues(typeof(ListType));
        //             foreach (ListType subtype in listTypes)
        //             {
        //                 DataTypesDropdownValues.Add(string.Concat(type, " of ", subtype));
        //             }
        //             continue;
        //         }

        //         DataTypesDropdownValues.Add(type.ToString());
        //     }
        // }
    }   
}