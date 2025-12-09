using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            if (Type.IsList)
            {
                _default = new List<object>();
                CopyList((IList)Value, (IList)_default);
            }
            else
            {
                _default = Value;
            }
        }

        public void OnSceneStop()
        {
            if (Type.IsList)
            {
                CopyList((IList)_default, (IList)Value);
            }
            else
            {
                Value = _default;
            }
        }

        public void SetName(string name)
        {
            Name = name;
            
            OnUpdated?.Invoke();
        }

        private void CopyList(IList a, IList b)
        {
            b.Clear();
            foreach (var item in a)
            {
                b.Add(item);
            }
        }
    }
}