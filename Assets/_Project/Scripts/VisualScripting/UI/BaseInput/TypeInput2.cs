using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class TypeInput2 : BaseInput
    {
        public TMP_Dropdown Dropdown;
        public Toggle ListToggle;

        private ScriptDataType _type = ScriptDataType.Single(DataType.Any);

        private DataType[] _types = new DataType[]
        {
            DataType.Any,
            DataType.String,
            DataType.Number,
            DataType.Boolean,
            DataType.Color,
            DataType.Entity,
        };
        private List<string> _dropdownValues;

        private void Awake()
        {
            _dropdownValues = _types.Select(s => s.ToString()).ToList();
            
            Dropdown.onValueChanged.AddListener(OnDropdownChanged);
            ListToggle.onValueChanged.AddListener(OnListToggleChanged);

            Dropdown.ClearOptions();
            Dropdown.AddOptions(_dropdownValues);
        }

        private void OnDropdownChanged(int index)
        {
            DataType dataType = _types[index];
            bool isList = _type.IsList;
            _type = new ScriptDataType(dataType, isList);
            
            OnSubmit?.Invoke(_type);
        }

        private void OnListToggleChanged(bool value)
        {
            DataType dataType = _type.Type;
            bool isList = value;
            _type = new ScriptDataType(dataType, isList);
            
            OnSubmit?.Invoke(_type);
        }

        public override void SetValue(object value)
        {
            _type = (ScriptDataType)value;

            int index = TypeToDropdownIndex(_type);
            Dropdown.SetValueWithoutNotify(index);
            ListToggle.SetIsOnWithoutNotify(_type.IsList);
        }

        public override object GetValue()
        {
            return _type;
        }

        private int TypeToDropdownIndex(ScriptDataType type)
        {
            return (int)type.Type;
        }
    }
}