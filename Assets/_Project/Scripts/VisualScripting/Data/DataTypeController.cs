using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Loykas.Scripting
{
    public class DataTypeController : Singleton<DataTypeController>
    {
        public List<DataType> DataTypes = new();
        public List<string> DataTypesDropdownValues = new();

        protected override void Awake()
        {
            base.Awake();

            InitDataTypes();
        }

        private void InitDataTypes()
        {
            var defaults = Enum.GetValues(typeof(DataType));

            foreach (DataType type in defaults)
            {
                if (type == DataType.Any)
                {
                    continue;
                }

                DataTypes.Add(type);

                if (type == DataType.List)
                {
                    var listTypes = Enum.GetValues(typeof(ListType));
                    foreach (ListType subtype in listTypes)
                    {
                        DataTypesDropdownValues.Add(string.Concat(type, " of ", subtype));
                    }
                    continue;
                }

                DataTypesDropdownValues.Add(type.ToString());
            }
        }
    }   
}