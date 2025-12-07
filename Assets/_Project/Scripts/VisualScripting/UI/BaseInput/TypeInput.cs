using System.Diagnostics;
using Loykas.Scripting;
using TMPro;

namespace Loykas.Scripting
{
    public class TypeInput : BaseInput
    {
        public TMP_Dropdown Dropdown;

        private ScriptDataType _type = DataTypeController.DataTypeList[0];

        private void Awake()
        {
            Dropdown.onValueChanged.AddListener(OnValueChanged);
            Dropdown.ClearOptions();
            Dropdown.AddOptions(DataTypeController.DataTypesDropdownValues);
        }

        private void OnValueChanged(int index)
        {
            _type = DataTypeController.DataTypeList[index];
            OnSubmit?.Invoke(_type);
        }

        public override void SetValue(object value)
        {
            _type = (ScriptDataType)value;

            int index = DataTypeController.TypeToDropdownIndex(_type);
            Dropdown.SetValueWithoutNotify(index);
        }

        public override object GetValue()
        {
            return _type;
        }
    }
}