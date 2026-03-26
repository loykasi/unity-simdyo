using UnityEngine;

namespace Loykas.Scripting
{
    class SetPositionNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue X;
        public InputValue Y;
        public InputValue Entity;

        public override ScriptNode Create()
        {
            return new SetPositionNode();
        }

        public SetPositionNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Set);
            Exit = CreateOutputTrigger(nameof(Exit));

            Entity = CreateInputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .UseInput()
                        .NullMeanSelf();

            X = CreateInputValue
            (
                nameof(X),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            ).UseInput();

            Y = CreateInputValue
            (
                nameof(Y),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            ).UseInput();
        }

        public OutputTrigger Set(NodeTask task)
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null)
            {
                return Exit;
            }

            float x = X.GetValue().NumberValue;
            float y = Y.GetValue().NumberValue;

            entity.Position = new Vector3(x, y, 0f);
            return Exit;
        }
    }
}