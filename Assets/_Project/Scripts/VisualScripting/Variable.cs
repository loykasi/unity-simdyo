using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Loykas.Scripting
{
    public class Variable
    {
        public event UnityAction OnUpdated;
        public event UnityAction OnTypeUpdated;

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

        public void Init(Variable variable)
        {
            Name = variable.Name;
            Type = variable.Type;
            Value = variable.Value;
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

        public void SetType(ScriptDataType type)
        {
            Type = type;
            OnTypeUpdated?.Invoke();
        }

        private void CopyList(IList a, IList b)
        {
            b.Clear();
            foreach (var item in a)
            {
                b.Add(item);
            }
        }

        public void Clear()
        {
            _default = null;
            Value = null;
            ScriptPool.Instance.Variable.Release(this);
        }

        public ValueTransfer GetValueTransfer()
        {
            return Value switch
            {
                string stringValue => ValueTransfer.CreateString(stringValue),
                float floatValue => ValueTransfer.CreateNumber(floatValue),
                int intValue => ValueTransfer.CreateNumber(intValue),
                bool boolValue => ValueTransfer.CreateBool(boolValue),
                ColorHSV colorValue => ValueTransfer.CreateColor(colorValue),
                SceneEntity entityValue => ValueTransfer.CreateEntity(entityValue),
                _ => default,
            };
        }
    }
}