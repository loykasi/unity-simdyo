using UnityEngine;

namespace Loykas.Scripting
{
    class TranslateNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Entity;
        public InputValue X;
        public InputValue Y;

        public override ScriptNode Create()
        {
            return new TranslateNode();
        }

        public TranslateNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Move);
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
            )
            .UseInput();

            Y = CreateInputValue
            (
                nameof(Y),
                ScriptDataType.Single(DataType.Number),
                new PortSettings
                {
                    IsLocalizationDisabled = true
                }
            )
            .UseInput();
        }

        public OutputTrigger Move(NodeTask task)
        {
            SceneEntity entity = Flow.GetEntity(Entity);
            if (entity == null)
            {
                return Exit;
            }

            float x = X.GetValue().NumberValue;
            float y = Y.GetValue().NumberValue;

            entity.Position += new Vector3(x, y, 0) * Time.deltaTime;

            return Exit;
        }
    }
}