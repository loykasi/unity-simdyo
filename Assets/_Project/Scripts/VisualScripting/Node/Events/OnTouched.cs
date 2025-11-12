using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Loykas.Scripting
{
    public class OnTouchedNode : EventNode
    {
        public OutputValue OtherEntity;
        public OutputValue PositionX;
        public OutputValue PositionY;
        // public OutputValue NormalXOutput;
        // public OutputValue NormalYOutput;

        private SceneEntity _otherEntity;
        //private Vector3 _normal;
        private Vector3 _position;

        public OnTouchedNode()
        {
            OtherEntity = OutputValue(nameof(OtherEntity), ScriptDataType.Single(DataType.Entity), GetOtherEntity);

            PositionY = OutputValue(nameof(PositionX), ScriptDataType.Single(DataType.Number), GetPositionX);
            PositionY = OutputValue(nameof(PositionY), ScriptDataType.Single(DataType.Number), GetPositionX);

            //NormalXOutput = OutputValue(nameof(EntityOutput), ScriptDataType.Single(DataType.Number), (flow) => _normal.x);
            //NormalYOutput = OutputValue(nameof(EntityOutput), ScriptDataType.Single(DataType.Number), (flow) => _normal.y);
        }

        public override EventHook Hook => EventHook.OnTouched;

        public object GetOtherEntity() => _otherEntity;
        public object GetPositionX() => _position.x;
        public object GetPositionY() => _position.y;

        public override void AssignArgument(object args)
        {
            var collision = (Collision2D)args;
            ContactPoint2D point = collision.GetContact(0);

            _otherEntity = collision.collider.GetComponent<SceneEntity>();
            _position = point.point;
            // _normal = point.normal;
        }
    }
}