namespace Loykas.Scripting
{
    public struct Key
    {
        public KeyCode Value;

        public Key(KeyCode key)
        {
            Value = key;
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