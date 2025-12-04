using System.Collections;
using UnityEngine;

namespace Loykas.Scripting
{
    class LogNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Debug;

        public InputTrigger Enter;
        public OutputTrigger Exit;
        public InputValue Value;

        public override ScriptNode Create()
        {
            return new LogNode();
        }

        public LogNode() : base()
        {
            Enter = CreateInputTrigger(nameof(Enter), Log);
            Exit = OutputTrigger(nameof(Exit));
            Value = InputValue(nameof(Value));
        }

        private OutputTrigger Log()
        {
            var value = Value.GetValue();
            LogCommand.Instance.Log(value);

            if (value is IList list)
            {
                string listValue = "";
                foreach (var item in list)
                {
                    listValue += item.ToString() + " | ";
                }
                Debug.Log($"list: {listValue}");
            }
            else
            {
                Debug.Log($"[{Time.frameCount}] {value}");
            }

            return Exit;
        }
    }
}