using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace Loykas.Scripting
{
    [ScriptNode(ScriptNodeCategory.Debug)]
    public class LogNodeContent : ScriptNodeContent
    {
        public override System.Type Type => typeof(LogNode);
        public override ScriptNode Create() => new LogNode();
    }

    class LogNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;
        public InputValue Value;

        public LogNode() : base()
        {
            Enter = InputTrigger(nameof(Enter), Log);
            Exit = OutputTrigger(nameof(Exit));
            Value = InputValue(nameof(Value));
        }

        private OutputTrigger Log(ScriptFlow vs)
        {
            var value = Value.GetValue(vs);
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
                Debug.Log(value);
            }

            return Exit;
        }
    }
}