using UnityEngine;
using UnityEngine.InputSystem;

namespace Loykas.Scripting
{
    public class OnKeyPressedNode : EventNode
    {
        public InputValue KeyCode;

        public override ScriptNode Create()
        {
            return new OnKeyPressedNode();
        }

        public OnKeyPressedNode()
        {
            KeyCode = CreateInputValue
            (
                nameof(KeyCode),
                ScriptDataType.Single(DataType.Key),
                new PortSettings
                {
                    IsConnectionDisabled = true
                }
            )
            .UseInput(InputValueTypes.Key);
        }

        public override EventHook Hook => EventHook.Update;

        public override bool ShouldTrigger()
        {
            Key keyCode = KeyCode.GetValue().KeyValue;

            if (keyCode.IsAny)
            {
                return Keyboard.current.anyKey.wasPressedThisFrame;
            }
            return Keyboard.current[keyCode.ToKey()].wasPressedThisFrame;
        }
    }
}