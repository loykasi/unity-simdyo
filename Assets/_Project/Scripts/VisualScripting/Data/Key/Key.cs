using System;

namespace Loykas.Scripting
{
    public struct Key
    {
        public KeyCode Value;

        public Key(KeyCode key)
        {
            Value = key;
        }

        public Key(string key)
        {
            if (Enum.TryParse(key, out KeyCode result))
            {
                Value = result;
            }
            else
            {
                Value = KeyCode.Any;
            }
        }

        public readonly bool IsAny => Value == KeyCode.Any;

        public readonly UnityEngine.InputSystem.Key ToKey()
        {
            return (UnityEngine.InputSystem.Key)(int)Value;
        }

        public override readonly string ToString()
        {
            return Value.ToString();
        }
    }
}