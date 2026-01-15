using UnityEngine;

namespace Loykas.Scripting
{
    class SendSignalNode : ScriptNode
    {
        public InputTrigger Enter;
        public OutputTrigger Exit;
        public InputValue Name;
        public InputValue Entity;

        public SendSignalNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), SendSignal);
            Exit = OutputTrigger(nameof(Exit));
            Name = InputValue(nameof(Name), ScriptDataType.Single(DataType.String))
                .UseInput()
                .HideLabel()
                .DisableConnection();
            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                .UseSignalEntityInput()
                .UseGlobalLocalized();
        }

        public override ScriptNode Create()
        {
            return new SendSignalNode();
        }

        private OutputTrigger SendSignal(NodeTask task)
        {
            SceneEntity entity = Flow.GetEntity(Entity);
            string signalName = (string)Name.GetValue();
            SignalSystem.Instance.SendSignal(signalName, entity);
            return Exit;
        }
    }
}