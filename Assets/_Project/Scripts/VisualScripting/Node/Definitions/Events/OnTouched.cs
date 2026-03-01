using System.Collections.Generic;
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

        private int _otherEntityId;
        //private Vector3 _normal;
        private Vector3 _position;

        public override ScriptNode Create()
        {
            return new OnTouchedNode();
        }

        public OnTouchedNode()
        {
            OtherEntity = OutputValue(nameof(OtherEntity), ScriptDataType.Single(DataType.Entity), GetOtherEntity);

            PositionY = OutputValue(nameof(PositionX), ScriptDataType.Single(DataType.Number), GetPositionX);
            PositionY = OutputValue(nameof(PositionY), ScriptDataType.Single(DataType.Number), GetPositionX);

            //NormalXOutput = OutputValue(nameof(EntityOutput), ScriptDataType.Single(DataType.Number), (flow) => _normal.x);
            //NormalYOutput = OutputValue(nameof(EntityOutput), ScriptDataType.Single(DataType.Number), (flow) => _normal.y);
        }

        public override EventHook Hook => EventHook.OnTouched;

        public object GetOtherEntity() => _otherEntityId;
        public object GetPositionX() => _position.x;
        public object GetPositionY() => _position.y;

        public override void AssignArgument(object value)
        {
            if (value is Collision2D collision2D)
            {
                ContactPoint2D point = collision2D.GetContact(0);

                _otherEntityId = collision2D.collider.GetComponent<SceneEntity>().Id;
                _position = point.point;
                // _normal = point.normal;
            }
            else if (value is Collider2D collider2D)
            {
                _otherEntityId = collider2D.GetComponent<SceneEntity>().Id;
                _position = Vector3.zero;
            }
        }
    }
}