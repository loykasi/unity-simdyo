using UnityEngine;

namespace Loykas.Scripting
{
    class GetColorNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Look;

        public InputValue Entity;
        public OutputValue Color;

        public override ScriptNode Create()
        {
            return new GetColorNode();
        }

        public GetColorNode()
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
                        
            Color = CreateOutputValue
            (
                nameof(Color),
                Get,
                ScriptDataType.Single(DataType.Color),
                new PortSettings
                {
                    LocalizationKey = nameof(Color)
                }
            );
        }

        public ValueTransfer Get()
        {
            SceneEntity entity = Flow.GetEntity(Entity);

            if (entity == null || entity is not MeshEntity meshEntity)
            {
                return ValueTransfer.CreateColor(default);
            }

            return ValueTransfer.CreateColor(meshEntity.CurrentColor);
        }
    }
}