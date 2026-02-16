using UnityEngine;

namespace Loykas.Scripting
{
    class MoveNode : ScriptNode
    {
        public override ScriptNodeCategory Category => ScriptNodeCategory.Motion;

        public InputTrigger Enter;
        public OutputTrigger Exit;

        public InputValue Entity;
        public InputValue X;
        public InputValue Y;
        public InputValue Duration;

        private bool _isFirstFrame = true;
        private float _time;
        private float _duration;
        private Vector3 _start;
        private Vector3 _target;

        public override ScriptNode Create()
        {
            return new MoveNode();
        }

        public MoveNode()
        {
            Enter = CreateInputTrigger(nameof(Enter), Move);
            Exit = OutputTrigger(nameof(Exit));

            Entity = InputValue(nameof(Entity), ScriptDataType.Single(DataType.Entity))
                        .HideLabel()
                        .UseInput()
                        .NullMeanSelf();

            X = InputValue(nameof(X), ScriptDataType.Single(DataType.Number)).UseInput().NoLocalize();
            Y = InputValue(nameof(Y), ScriptDataType.Single(DataType.Number)).UseInput().NoLocalize();
            Duration = InputValue(nameof(Duration), ScriptDataType.Single(DataType.Number)).UseInput().NoLocalize();
        }

        public OutputTrigger Move(NodeTask task)
        {
            SceneEntity entity = Flow.GetEntity(Entity);
            if (entity == null)
            {
                return Exit;
            }

            if (_isFirstFrame)
            {
                _duration = (float)Duration.GetValue();
                _time = _duration;
                _isFirstFrame = false;

                _start = entity.Position;
                _target.x = (float)X.GetValue();
                _target.y = (float)Y.GetValue();
            }

            if (_time > 0)
            {
                _time -= Time.deltaTime;
                
                entity.Position = Vector3.Lerp(_target, _start , _time / _duration);

                return null;
            }
            _isFirstFrame = true;
            entity.Position = _target;

            return Exit;
        }
    }
}