using UnityEngine;

namespace Loykas.Scripting
{
    class GetGravityNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputValue Entity;
        public OutputValue Value;

        public GetGravityNode()
        {
            Entity = CreateInputValue
            (
                nameof(Entity),
                ScriptDataType.Single(DataType.Entity),
                new PortSettings
                {
                    HideLabel = true
                }
            )
            .UseInput()
            .NullMeanSelf();
                        
            Value = CreateOutputValue
            (
                nameof(Value),
                Get,
                ScriptDataType.Single(DataType.Boolean),
                new PortSettings
                {
                    LocalizationKey = nameof(Value)
                }
            );
        }

        public override ScriptNode Create()
        {
            return new GetGravityNode();
        }

        public ValueTransfer Get()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null || entity is not MeshEntity meshEntity)
            {
                return ValueTransfer.CreateBool(default);
            }

            return ValueTransfer.CreateBool(meshEntity.IsGravityEnabled);
        }
    }
}