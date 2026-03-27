using UnityEngine;

namespace Loykas.Scripting
{
    class CreateCloneNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Control;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Entity;
        public OutputValue EntityOutput;

        private SceneEntity _entity;

        public override ScriptNode Create()
        {
            return new CreateCloneNode();
        }

        public override void Build()
        {
            Enter = CreateInputTrigger(nameof(Enter), Clone);
            Exit = CreateOutputTrigger(nameof(Exit), new PortSettings
            {
                HideLabel = true
            });

            Entity = CreateInputValue
            (
                nameof(Entity),
                ScriptDataType.Single(DataType.Entity),
                new PortSettings
                {
                    HideLabel = true,
                }
            )
            .UseInput()
            .NullMeanSelf();

            EntityOutput = CreateOutputValue
            (
                nameof(EntityOutput),
                Get,
                ScriptDataType.Single(DataType.Entity),
                PortSettings.Default
            );
        }

        private OutputTrigger Clone(NodeTask task)
        {
            SceneEntity entity = Flow.GetEntity(Entity);
            _entity = entity.CloneEntity();
            if (_entity == null)
            {
                return Exit;
            }

            _entity.Script.TriggerEvent(EventHook.StartAsClone);
            _entity.OnStart();

            return Exit;
        }

        private ValueTransfer Get()
        {
            return ValueTransfer.CreateEntity(_entity);
        }
    }
}