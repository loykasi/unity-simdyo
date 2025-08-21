using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loykas.Scripting
{
    public class DataTypeController : Singleton<DataTypeController>
    {
        public List<ScriptDataType> DataTypes = new();
        public List<string> DataTypesDropdownValues = new();

        protected override void Awake()
        {
            base.Awake();

            InitDataTypes();
        }

        private void InitDataTypes()
        {
            // load all default from enum
            var defaults = Enum.GetValues(typeof(DataType));

            foreach (DataType type in defaults)
            {
                if (type == DataType.Any)
                {
                    continue;
                }

                if (type == DataType.List)
                {
                    var listTypes = Enum.GetValues(typeof(ListType));
                    foreach (ListType subtype in listTypes)
                    {
                        DataTypes.Add
                        (
                            new ScriptDataType(type.ToString(), subtype.ToString())
                        );
                    }

                    continue;
                }

                DataTypes.Add
                (
                    new ScriptDataType(type.ToString())
                );
            }

            DataTypesDropdownValues = DataTypes.Select(t => t.ToString()).ToList();
        }
    }   
}