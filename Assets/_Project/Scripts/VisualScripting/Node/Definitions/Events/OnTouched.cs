using UnityEngine;

namespace Loykas.Scripting
{
    public class OnTouchedNode : EventNode
    {
        public override bool CanUseGlobal => false;

        public OutputValue OtherEntity;
        public OutputValue PositionX;
        public OutputValue PositionY;
        // public OutputValue NormalXOutput;
        // public OutputValue NormalYOutput;
        
        private SceneEntity _entity;
        //private Vector3 _normal;
        private Vector3 _position;

        public override ScriptNode Create()
        {
            return new OnTouchedNode();
        }

        public override void Build()
        {
            base.Build();
            
            OtherEntity = CreateOutputValue
            (
                nameof(OtherEntity),
                GetOtherEntity,
                ScriptDataType.Single(DataType.Entity),
                PortSettings.Default
            );

            PositionY = CreateOutputValue
            (
                nameof(PositionX),
                GetPositionX,
                ScriptDataType.Single(DataType.Number),
                PortSettings.Default
            );
            PositionY = CreateOutputValue
            (
                nameof(PositionY),
                GetPositionX,
                ScriptDataType.Single(DataType.Number),
                PortSettings.Default
            );

            //NormalXOutput = OutputValue(nameof(EntityOutput), ScriptDataType.Single(DataType.Number), (flow) => _normal.x);
            //NormalYOutput = OutputValue(nameof(EntityOutput), ScriptDataType.Single(DataType.Number), (flow) => _normal.y);
        }

        public override EventHook Hook => EventHook.OnTouched;

        public ValueTransfer GetOtherEntity()
        {
            return ValueTransfer.CreateEntity(_entity);
        }

        public ValueTransfer GetPositionX()
        {
            return ValueTransfer.CreateNumber(_position.x);
        }

        public ValueTransfer GetPositionY()
        {
            return ValueTransfer.CreateNumber(_position.y);
        }

        public override void AssignArgument(object value)
        {
            if (value is Collision2D collision2D)
            {
                ContactPoint2D point = collision2D.GetContact(0);

                _entity = collision2D.collider.GetComponent<SceneEntity>();
                _position = point.point;
                // _normal = point.normal;
            }
            else if (value is Collider2D collider2D)
            {
                _entity = collider2D.GetComponent<SceneEntity>();
                _position = Vector3.zero;
            }
        }
    }
}