using UnityEngine;
using UnityEngine.InputSystem;

namespace Loykas.Scripting
{
    public class OnKeyPressedNode : EventNode
    {
        public InputValue KeyCode;

        public OnKeyPressedNode(string title) : base(title)
        {
            KeyCode = InputValue(nameof(KeyCode)).UseKeyCodeInput().DisableConnection();
        }

        public override EventHook Hook => EventHook.Update;

        public override bool ShouldTrigger()
        {
            var keyCode = (Key)KeyCode.GetValue(Flow);

            if (keyCode.IsAny)
            {
                return Keyboard.current.anyKey.wasPressedThisFrame;
            }
            return Keyboard.current[keyCode.ToKey()].wasPressedThisFrame;
        }
    }
}