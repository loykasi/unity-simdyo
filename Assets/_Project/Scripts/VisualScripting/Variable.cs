using UnityEngine.Events;

namespace Loykas.Scripting
{
    public class Variable
    {
        public UnityAction OnUpdated;

        public string Name;
        public ScriptDataType Type;

        public object Value;
        private object _default;

        public Variable()
        {
            Type = ScriptDataType.Default();
            Value = string.Empty;
        }

        public Variable(ScriptDataType type, object value)
        {
            Type = type;
            Value = value;
        }

        public void OnSceneStart()
        {
            _default = Value;
        }

        public void OnSceneStop()
        {
            Value = _default;
        }

        public void SetName(string name)
        {
            Name = name;
            
            OnUpdated?.Invoke();
        }
    }
}