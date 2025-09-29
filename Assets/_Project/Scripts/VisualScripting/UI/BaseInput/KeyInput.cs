using TMPro;

namespace Loykas.Scripting
{
    public class KeyInput : BaseInput
    {
        public TMP_Dropdown Dropdown;

        private Key _key;

        private void Awake()
        {
            Dropdown.onValueChanged.AddListener(OnValueChanged);
            Dropdown.ClearOptions();
            Dropdown.AddOptions(KeyHandler.KeyOptions);
        }

        private void OnValueChanged(int index)
        {
            _key = KeyHandler.ToKey(index);
            OnSubmit?.Invoke(_key);
        }

        public override void SetValue(object value)
        {
            _key = (Key)value;
            int index = KeyHandler.ToIndex(_key);
            Dropdown.SetValueWithoutNotify(index);
        }

        public override object GetValue()
        {
            return _key;
        }
    }
}