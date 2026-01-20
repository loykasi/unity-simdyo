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
            Exit = OutputTrigger(nameof(Exit));

            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();

            X = InputValue(nameof(X), ScriptDataType.Single(DataType.Number)).UseInput().NoLocalize();
            Y = InputValue(nameof(Y), ScriptDataType.Single(DataType.Number)).UseInput().NoLocalize();
        }

        public OutputTrigger Move(NodeTask task)
        {
            SceneEntity entity = Flow.GetEntity(Entity);
            if (entity == null)
            {
                return Exit;
            }

            float x = (float)X.GetValue();
            float y = (float)Y.GetValue();

            entity.Position += new Vector3(x, y, 0) * Time.deltaTime;

            return Exit;
        }
    }
}