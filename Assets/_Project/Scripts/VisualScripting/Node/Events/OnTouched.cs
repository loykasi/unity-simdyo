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

        public OnTouchedNode(string title) : base(title)
        {
            OtherEntity = OutputValue(nameof(OtherEntity), ScriptDataType.Single(DataType.Entity), (flow) => _otherEntity);

            PositionY = OutputValue(nameof(PositionX), ScriptDataType.Single(DataType.Number), (flow) => _position.x);
            PositionY = OutputValue(nameof(PositionY), ScriptDataType.Single(DataType.Number), (flow) => _position.y);

            //NormalXOutput = OutputValue(nameof(EntityOutput), ScriptDataType.Single(DataType.Number), (flow) => _normal.x);
            //NormalYOutput = OutputValue(nameof(EntityOutput), ScriptDataType.Single(DataType.Number), (flow) => _normal.y);
        }

        public override EventHook Hook => EventHook.OnTouched;

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