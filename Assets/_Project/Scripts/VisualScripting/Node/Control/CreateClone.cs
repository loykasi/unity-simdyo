using Unity.VisualScripting;
using UnityEngine;

namespace Loykas.Scripting
{
    class CreateCloneNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Control;
        public override bool CanUseGlobal => false;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Entity;
        public OutputValue EntityOutput;

        private SceneEntity _entity;

        public override ScriptNode Create()
        {
            return new CreateCloneNode();
        }

        public CreateCloneNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Clone);
            Exit = OutputTrigger(nameof(Exit)).HideLabel();

            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();

            EntityOutput = OutputValue(nameof(EntityOutput), ScriptDataType.Single(DataType.Entity), Get);
        }

        private OutputTrigger Clone()
        {
            _entity = Flow.Entity.CloneEntity();
            _entity.Script.TriggerEvent(EventHook.StartAsClone);
            _entity.OnStart();

            Debug.Log("Create " + _entity.Id);

            return Exit;
        }

        private object Get()
        {
            return _entity.Id;
        }
    }
}