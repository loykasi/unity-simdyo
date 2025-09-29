using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace Loykas.Scripting
{
    [CreateAssetMenu(fileName = "LogNode", menuName = "Scriptable Objects/Visual Scripting/Node/Log")]
    public class Log : ScriptNodeData
    {
        public override ScriptNode Create()
        {
            return new LogNode(Title);
        }
    }

    class LogNode : ScriptNode
    {
        [JsonIgnore]
        public InputTrigger Enter;
        [JsonIgnore]
        public OutputTrigger Exit;
        [JsonIgnore]
        public InputValue Value;

        public LogNode(string title) : base(title)
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